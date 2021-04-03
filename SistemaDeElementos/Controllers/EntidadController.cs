using Microsoft.AspNetCore.Mvc;
using System;
using Gestor.Errores;
using GestorDeElementos;
using UtilidadesParaIu;
using System.Collections.Generic;
using Utilidades;
using Newtonsoft.Json;
using MVCSistemaDeElementos.Descriptores;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using System.Reflection;
using ModeloDeDto;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Authorization;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Negocio;

namespace MVCSistemaDeElementos.Controllers
{

    enum epAcciones { buscar, siguiente, anterior, ultima, ordenar }

    [Authorize]
    public class EntidadController<TContexto, TRegistro, TElemento> : BaseController<TElemento>
        where TContexto : ContextoSe
        where TRegistro : Registro
        where TElemento : ElementoDto
    {

        protected GestorDeElementos<TContexto, TRegistro, TElemento> GestorDeElementos { get; }


        public EntidadController(GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores, DescriptorDeCrud<TElemento> descriptor)
        : this(gestorDeElementos, gestorErrores)
        {
            Descriptor = descriptor;
            if (NegociosDeSe.ParsearDto(typeof(TElemento).Name) != enumNegocio.No_Definido)
                Descriptor.negocioDtm =  GestorDeNegocios.LeerNegocio(GestorDeElementos.Contexto, NegociosDeSe.ParsearDto(typeof(TElemento).Name));

            var gestorDeVista = GestorDeVistaMvc.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            var vista = gestorDeVista.LeerVistaMvc($"{Descriptor.Controlador}.{Descriptor.Vista}");

            Descriptor.Creador.AbrirEnModal = vista.MostrarEnModal;
            Descriptor.Editor.AbrirEnModal = vista.MostrarEnModal;
        }


        public EntidadController(GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores)
        : base(gestorErrores, gestorDeElementos.Contexto.DatosDeConexion)
        {
            GestorDeElementos = gestorDeElementos;
            GestorDeElementos.Contexto.IniciarTraza();
        }


        protected override void Dispose(bool disposing)
        {
            GestorDeElementos.Contexto.CerrarTraza();
            base.Dispose(disposing);
        }

        //Llamada desde opciones de menu (Menu.Ts)
        public IActionResult Index()
        {
            return RedirectToAction(Descriptor.Vista);
        }

        /// <summary>
        /// END-POIN: desde el ApiDeArchivos. Sube un fichero al gestor documental o a la ruta indicada
        /// </summary>
        /// <param name="fichero">fichero a subir</param>
        /// <param name="rutaDestino">si no se sube al gestor documenta, nombre de la ruta donde se almacenará</param>
        /// <param name="extensionesValidas">extensiones que ha de tener el archivo a subir</param>
        /// <returns>0 si no ha subido al gestor documental, o id del archivo subido al gestor documental</returns>
        [HttpPost]
        public JsonResult epSubirArchivo(IFormFile fichero, string rutaDestino, string extensionesValidas)
        {
            return SubirArchivo(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext, fichero, rutaDestino, extensionesValidas);
        }

        //END-POINT: Desde CrudCreacion.ts
        public JsonResult epCrearElemento(string elementoJson)
        {
            var r = new Resultado();
            var tran = GestorDeElementos.IniciarTransaccion();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                var elemento = JsonConvert.DeserializeObject<TElemento>(elementoJson);
                AntesDeEjecutar_CrearElemento(elementoJson);
                GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Registro creado";
                GestorDeElementos.Commit(tran);
            }
            catch (Exception e)
            {
                GestorDeElementos.Rollback(tran);
                ApiController.PrepararError(e, r, "No se ha podido crear.");
            }

            return new JsonResult(r);
        }

        protected virtual void AntesDeEjecutar_CrearElemento(string elementoJson)
        {
        }

        //END-POINT: Desde CrudEdicion.ts
        public JsonResult epModificarPorId(string elementoJson)
        {
            var r = new Resultado();
            var tran = GestorDeElementos.IniciarTransaccion();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                var elemento = JsonConvert.DeserializeObject<TElemento>(elementoJson);
                AntesDeEjecutar_ModificarPorId(elementoJson);
                GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Registro modificado";
                GestorDeElementos.Commit(tran);
            }
            catch (Exception e)
            {
                GestorDeElementos.Rollback(tran);
                ApiController.PrepararError(e, r, "No se ha podido modificar.");
            }

            return new JsonResult(r);
        }

        protected virtual void AntesDeEjecutar_ModificarPorId(string elementoJson)
        {
            
        }


        //END-POINT: Desde CrudEdicion.ts
        public JsonResult epLeerPorId(int id)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                var opcionesDeMapeo = new Dictionary<string, object>();
                opcionesDeMapeo.Add(ElementoDto.DescargarGestionDocumental, true);

                var elemento = GestorDeElementos.LeerElementoPorId(id, opcionesDeMapeo);
                var modoDeAcceso = GestorDeElementos.LeerModoDeAccesoAlElemento(DatosDeConexion.IdUsuario, NegociosDeSe.ParsearDto(elemento.GetType().Name), id);
                if (modoDeAcceso == enumModoDeAccesoDeDatos.SinPermiso)
                    GestorDeErrores.Emitir("El usuario conectado no tiene acceso al elemento solicitado");

                r.Datos = elemento;
                r.ModoDeAcceso = modoDeAcceso.Render(); 
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = $"registro leido";
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "Error al leer.");
            }

            return new JsonResult(r);

        }


        //END-POINT: Desde Grid.ts
        public JsonResult epBorrarPorId(string idsJson)
        {
            var r = new Resultado();

            var tran = GestorDeElementos.IniciarTransaccion();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                foreach (var id in listaIds)
                {
                    var elemento = GestorDeElementos.LeerElementoPorId(id);
                    GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(enumTipoOperacion.Eliminar));
                }
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = listaIds.Count > 1 ? "Registros eliminados" : "Registro eliminado";
                GestorDeElementos.Commit(tran);
            }
            catch (Exception e)
            {
                GestorDeElementos.Rollback(tran);
                ApiController.PrepararError(e, r, "No se ha podido eliminar.");
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde GridDeDatos.ts
        public JsonResult epLeerDatosParaElGrid(string modo, string accion, string posicion, string cantidad, string filtro, string orden)
        {
            var r = new Resultado();
            int pos = posicion.Entero();
            int can = cantidad.Entero();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);

                var datos = ApiController.LeerDatosParaElGrid(
                    () => Leer(pos, can, filtro, orden)
                  , () => accion == epAcciones.buscar.ToString() ? Contar(filtro) : Recontar(filtro));


                var infoObtenida = new ResultadoDeLectura();
                infoObtenida.total = datos.total;
                infoObtenida.registros = ElementosLeidos(datos.elementos.ToList());
                infoObtenida.posicion = pos;
                infoObtenida.cantidad = can;

                r.Datos = infoObtenida;
                r.Estado = enumEstadoPeticion.Ok;
                if (pos > 0 && datos.elementos.Count() == 0)
                    r.Mensaje = "No hay más elementos";
            }
            catch (Exception e)
            {
              ApiController.PrepararError(e,r, "No se ha podido recuperar datos para el grid.");
            }

            var a = new JsonResult(r);
            return a;
        }

        private List<Dictionary<string, object>> ElementosLeidos(List<TElemento> elementos)
        {
            var listaDeElementos = new List<Dictionary<string, object>>();
            if (elementos.Count > 0)
            {
                PropertyInfo[] propiedades = elementos[0].GetType().GetProperties();

                foreach (TElemento elemento in elementos)
                {
                    var registro = new Dictionary<string, object>();
                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        object valor = elemento.GetType().GetProperty(propiedad.Name).GetValue(elemento);
                        registro[propiedad.Name] = valor == null ? "" : valor;
                    }
                    var ma = GestorDeElementos.LeerModoDeAccesoAlElemento(DatosDeConexion.IdUsuario, NegociosDeSe.ParsearDto(elemento.GetType().Name), registro[nameof(ElementoDto.Id)].ToString().Entero());
                    registro[nameof(Resultado.ModoDeAcceso)] = ma.Render();
                    listaDeElementos.Add(registro);
                }
            }

            return listaDeElementos;
        }


        //END-POINT: Desde ModalSeleccion.ts
        public JsonResult epLeerParaSelector(string filtro)
        {
            var r = new Resultado();
            List<TElemento> elementos;
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                elementos = Leer(0, 2, filtro, null).ToList();
                r.Datos = elementos;
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e,r, "No se ha podido leer los datos.");
            }

            return new JsonResult(r);
        }


        /// <summary>
        /// END-POINT: Desde CrudBase.ts
        /// llama al metodo del controlador CargarLista y en función de la claseElemento obtiene que elementos ha de cargar
        /// </summary>
        /// <param name="claseElemento">Indica la lista de elementos que se quiere cargar</param>
        /// <returns></returns>
        public JsonResult epCargarLista(string claseElemento)
        {
            var r = new Resultado();
            dynamic elementos;
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                elementos = CargarLista(claseElemento);
                r.Datos = elementos;
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "No se ha podido leer los datos.");
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde CrudBase.ts
        public JsonResult epCargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            var r = new Resultado();
            dynamic elementos;
            try
            {
                ClausulaDeFiltrado clausula = filtro == null ? new ClausulaDeFiltrado() : JsonConvert.DeserializeObject<ClausulaDeFiltrado>(filtro);

                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                elementos = CargaDinamica(claseElemento, posicion, cantidad, clausula);
                r.Datos = elementos;
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "No se ha podido leer los datos.");
            }

            return new JsonResult(r);
        }


        //END-POINT: Desde ModalParaRelacionar.ts
        /// <summary>
        /// crea las relaciones entre el id de un elemento pasado y la lista de id's de otros elementos
        /// </summary>
        /// <param name="id">id del elemento pasado</param>
        /// <param name="idsJson">lista de ids en formato json de los ids con los que relacionar</param>
        /// <returns></returns>
        public JsonResult epCrearRelaciones(int id, string idsJson)
        {
            var r = new Resultado();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                var relacionados = 0;
                var mensajeInformativo = "";
                foreach (var idParaRelacionar in listaIds)
                {
                    string mensaje = GestorDeElementos.CrearRelacion(id, idParaRelacionar);
                    if (mensaje.IsNullOrEmpty())
                        relacionados++;
                    else
                        mensajeInformativo = mensajeInformativo + Environment.NewLine + mensaje;
                }
                r.Total = relacionados;
                r.consola = mensajeInformativo;
                r.Mensaje = $"Se han relacionado {relacionados} de los {listaIds.Count} marcados";
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "Error en el proceso de relación.");
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde CrudMantenimiento.ts
        /// <summary>
        /// Devuelve el modo de acceso a los datos del negocio del usuario conectado
        /// </summary>
        /// <param name="negocio">negocio del que se quiere saber el modo de acceso del usuario conectado</param>
        /// <returns>modo de acceso a los datos del negocio</returns>
        public JsonResult epLeerModoDeAccesoAlNegocio(string negocio)
        {
            var r = new Resultado();
            try
            {
                var modoDeAcceso = enumModoDeAccesoDeDatos.SinPermiso;
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                modoDeAcceso = GestorDeElementos.LeerModoDeAccesoAlNegocio(DatosDeConexion.IdUsuario, NegociosDeSe.Negocio(negocio));
                r.ModoDeAcceso = modoDeAcceso.Render();
                r.consola = $"El usuario {DatosDeConexion.Login} tiene permisos de {modoDeAcceso}";
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, $"Error al obtener los permisos sobre el negocio {negocio} para el usuario {DatosDeConexion.Login}.");
            }

            return new JsonResult(r);
        }

        public JsonResult epLeerModoDeAccesoAlElemento(string negocio, int id)
        {
            var r = new Resultado();
            try
            {
                var modoDeAcceso = enumModoDeAccesoDeDatos.SinPermiso;
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                var opcionesDeMapeo = new Dictionary<string, object>();
                opcionesDeMapeo.Add(ElementoDto.DescargarGestionDocumental, false);

                var elemento = GestorDeElementos.LeerElementoPorId(id, opcionesDeMapeo);
                modoDeAcceso = GestorDeElementos.LeerModoDeAccesoAlElemento(DatosDeConexion.IdUsuario, NegociosDeSe.Negocio(negocio), id);
                if (modoDeAcceso == enumModoDeAccesoDeDatos.SinPermiso)
                    GestorDeErrores.Emitir("El usuario conectado no tiene acceso al elemento solicitado");

                r.Datos = elemento;
                r.ModoDeAcceso = modoDeAcceso.Render();
                r.consola = $"El usuario {DatosDeConexion.Login} tiene permisos de {modoDeAcceso} sobre el elemento seleccionado";
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, $"Error al obtener los permisos sobre el elemento {id} del {negocio} para el usuario {DatosDeConexion.Login}.");
            }

            return new JsonResult(r);
        }

        protected virtual dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            throw new Exception($"Debe implementar la función de CargaDinamica para la clase '{claseElemento}' en el controlador '{this.GetType().Name}'");
        }

        protected virtual dynamic CargarLista(string claseElemento)
        {
            throw new NotImplementedException();
        }

        public ViewResult ViewCrud()
        {
            ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador,HttpContext);
            Descriptor.GestorDeUsuario = GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            Descriptor.UsuarioConectado = Descriptor.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);

            var destino = $"{(Descriptor.RutaBase.IsNullOrEmpty() ? "" : $"../{Descriptor.RutaBase}/")}{Descriptor.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
            string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();

            if (!Descriptor.UsuarioConectado.EsAdministrador)
            {
                var hayPermisos = Descriptor.GestorDeUsuario.TienePermisoFuncional(Descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    return RenderMensaje($"Solicite permisos de acceso a {destino}"); 

                hayPermisos = Descriptor.GestorDeUsuario.TienePermisoDeDatos(Descriptor.UsuarioConectado, enumModoDeAccesoDeDatos.Consultor, Descriptor.Negocio);
                if (!hayPermisos)
                    return RenderMensaje($"Solicite al menos permisos de consulta sobre los elementos de negocio {NegociosDeSe.ToString(Descriptor.Negocio)}");
            }


            Descriptor.GestorDeNegocio = GestorDeNegocios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            ViewBag.DatosDeConexion = DatosDeConexion;

            return base.View(destino, Descriptor);
        }

        public ViewResult ViewCrud<T>(DescriptorDeCrud<T> descriptor)
        where T : ElementoDto
        {
            ViewBag.DatosDeConexion = DatosDeConexion;
            return base.View(descriptor.Vista, descriptor);
        }

        /*
        protected IEnumerable<TElemento> LeerOrdenados(string filtro, string orden, string parametrosDeMapeo)
        {

            List<ClausulaDeFiltrado> filtros = filtro.IsNullOrEmpty()
                                               ? new List<ClausulaDeFiltrado>()
                                               : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);

            Dictionary<string, object> parametros = parametrosDeMapeo.IsNullOrEmpty()
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(parametrosDeMapeo);

            var elementos = GestorDeElementos.LeerElementos(Descriptor.Mnt.Datos.PosicionInicial
                                                          , Descriptor.Mnt.Datos.CantidadPorLeer
                                                          , filtros
                                                          , orden.ParsearOrdenacion()
                                                          , parametros);

            return elementos;
        }
        */

        public int Contar(string filtro = null)
        {
            List<ClausulaDeFiltrado> filtros = filtro.IsNullOrEmpty()
                                               ? new List<ClausulaDeFiltrado>()
                                               : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);

            return GestorDeElementos.Contar(filtros);
        }


        public int Recontar(string filtro = null)
        {
            List<ClausulaDeFiltrado> filtros = filtro.IsNullOrEmpty()
                                               ? new List<ClausulaDeFiltrado>()
                                               : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);

            return GestorDeElementos.Recontar(filtros);
        }

        protected IEnumerable<TElemento> Leer(int posicion, int cantidad, string filtro, string orden)
        {
            Descriptor.Mnt.Datos.CantidadPorLeer = cantidad;
            Descriptor.Mnt.Datos.PosicionInicial = posicion;

            var opcionesDeMapeo = new Dictionary<string, object>();
            opcionesDeMapeo.Add(ElementoDto.DescargarGestionDocumental, false);

            List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
            List<ClausulaDeOrdenacion> ordenes = orden == null ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(orden);

            if (ordenes.Count == 0 && typeof(TRegistro).ImplementaNombre())
                ordenes.Add(new ClausulaDeOrdenacion() { OrdenarPor = nameof(INombre.Nombre), Modo = ModoDeOrdenancion.ascendente });

            return GestorDeElementos.LeerElementos(posicion, cantidad, filtros, ordenes, opcionesDeMapeo);
        }

    }

}

