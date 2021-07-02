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
using GestoresDeNegocio.Archivos;
using Enumerados;

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


        public EntidadController(GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores)
        : base(gestorErrores, gestorDeElementos.Contexto, gestorDeElementos.Mapeador)
        {
            GestorDeElementos = gestorDeElementos;

        }


        ////Llamada desde opciones de menu (Menu.Ts)
        //public IActionResult Index()
        //{
        //    return RedirectToAction(Descriptor.Vista);
        //}

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
                var parametros = AntesDeEjecutar_CrearElemento(elemento);
                GestorDeElementos.PersistirElementoDto(elemento, parametros);
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

        protected virtual ParametrosDeNegocio AntesDeEjecutar_CrearElemento(TElemento elemento)
        {
            return new ParametrosDeNegocio(enumTipoOperacion.Insertar);
        }

        protected override TElemento LeerPorId(int id, Dictionary<string, object> parametros)
        {
            parametros.Add(ltrParametrosDto.DescargarGestionDocumental, true);
            parametros.Add(ltrParametrosDto.solicitadoPorLaCola, false);
            return GestorDeElementos.LeerElementoPorId(id, parametros);
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
                var p = AntesDeEjecutar_ModificarPorId(elemento);
                GestorDeElementos.PersistirElementoDto(elemento, p);
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

        protected virtual ParametrosDeNegocio AntesDeEjecutar_ModificarPorId(TElemento elemento)
        {
            return new ParametrosDeNegocio(enumTipoOperacion.Modificar);
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
                    var p = AntesDeEjecutar_BorrarPorId(elemento);
                    GestorDeElementos.PersistirElementoDto(elemento, p);
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

        protected virtual ParametrosDeNegocio AntesDeEjecutar_BorrarPorId(TElemento elemento)
        {
            return new ParametrosDeNegocio(enumTipoOperacion.Eliminar);
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

                //GestorDocumental.GenerarExcel(Contexto, datos.elementos.ToList());
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
                ApiController.PrepararError(e, r, "No se ha podido recuperar datos para el grid.");
            }

            var a = new JsonResult(r);
            return a;
        }

        //END-POINT: desde CrudMantenimiento.Ts
        public JsonResult epExportar(string parametrosJson = null)
        {
            var r = new Resultado();
            Dictionary<string, object> parametros = parametrosJson.ToDiccionarioDeParametros();
            parametros[nameof(ElementoDto)] = typeof(TElemento).Name;
            parametros[nameof(Registro)] = typeof(TRegistro).Name;

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);

                if (parametros.ContainsKey("sometido") && bool.Parse(parametros["sometido"].ToString()))
                {
                    GestorDocumental.SometerExportacion(Contexto, parametros.ToJson());
                    r.Mensaje = $"Trabajo sometido correctamente";
                }
                else
                {
                    var opcionesDeMapeo = new Dictionary<string, object>();
                    opcionesDeMapeo.Add(ltrParametrosDto.DescargarGestionDocumental, false);
                    var cantidad = !parametros.ContainsKey(ltrFiltros.cantidad) ? -1 : parametros[ltrFiltros.cantidad].ToString().Entero();
                    var posicion = !parametros.ContainsKey(ltrFiltros.posicion) ? 0 : parametros[ltrFiltros.posicion].ToString().Entero();
                    List<ClausulaDeFiltrado> filtros = !parametros.ContainsKey(ltrFiltros.filtro) || parametros[ltrFiltros.filtro].ToString().IsNullOrEmpty() ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(parametros["filtro"].ToString());
                    List<ClausulaDeOrdenacion> orden = !parametros.ContainsKey(ltrFiltros.orden) || parametros[ltrFiltros.orden].ToString().IsNullOrEmpty() ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(parametros["orden"].ToString());

                    var elementos = GestorDeElementos.LeerElementos(posicion, cantidad, filtros, orden, opcionesDeMapeo);
                    r.Datos = GestorDocumental.DescargarExcel(Contexto, elementos.ToList());
                    r.Mensaje = $"Exportado";
                }
                r.ModoDeAcceso = enumModoDeAccesoDeDatos.Consultor.Render();
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "Error al exportar.");
            }
            return new JsonResult(r);
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
                    var ma = GestorDeElementos.LeerModoDeAccesoAlElemento(DatosDeConexion.IdUsuario, NegociosDeSe.NegocioDeUnDto(elemento.GetType().FullName), registro[nameof(ElementoDto.Id)].ToString().Entero());
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
                ApiController.PrepararError(e, r, "No se ha podido leer los datos.");
            }

            return new JsonResult(r);
        }


        /// <summary>
        /// END-POINT: Desde CrudBase.ts
        /// llama al metodo del controlador CargarLista y en función de la claseElemento obtiene que elementos ha de cargar
        /// </summary>
        /// <param name="claseElemento">Indica la lista de elementos que se quiere cargar</param>
        /// <returns></returns>
        public JsonResult epCargarLista(string claseElemento, string negocio, string filtro)
        {
            var r = new Resultado();
            dynamic elementos;
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
                elementos = CargarLista(claseElemento, NegociosDeSe.ToEnumerado(negocio, nullValido: true), filtros);
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
        public JsonResult epCargaDinamica(string claseElemento, int posicion, int cantidad, string filtrosJson)
        {
            var r = new Resultado();
            dynamic elementos;
            try
            {
                List<ClausulaDeFiltrado> filtros = filtrosJson == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtrosJson);

                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                elementos = CargaDinamica(claseElemento, posicion, cantidad, filtros);
                r.Datos = elementos;
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "No se ha podido leer los datos.");
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
                opcionesDeMapeo.Add(ltrParametrosDto.DescargarGestionDocumental, false);

                var elemento = GestorDeElementos.LeerElementoPorId(id, opcionesDeMapeo);
                modoDeAcceso = GestorDeElementos.LeerModoDeAccesoAlElemento(DatosDeConexion.IdUsuario, NegociosDeSe.ToEnumerado(negocio), id);
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

        protected virtual dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            throw new Exception($"Debe implementar la función de CargaDinamica para la clase '{claseElemento}' en el controlador '{this.GetType().Name}'");
        }

        protected virtual dynamic CargarLista(string claseElemento, enumNegocio negocio, List<ClausulaDeFiltrado> filtros)
        {
            //if (claseElemento == nameof(ExportacionDto))
            //    return GestorDeExportaciones.LeerTipos(Contexto, claseElemento, negocio, filtros);

            throw new Exception($"Debe implementar la función de CargaDeElementos para la clase '{claseElemento}' en el controlador '{GetType().Name}'");
        }

        public ViewResult ViewCrud(DescriptorDeCrud<TElemento> descriptor)
        {
            if (NegociosDeSe.NegocioDeUnDto(typeof(TElemento).FullName) != enumNegocio.No_Definido)
                descriptor.negocioDtm = GestorDeNegocios.LeerNegocio(GestorDeElementos.Contexto, NegociosDeSe.NegocioDeUnDto(typeof(TElemento).FullName));

            var gestorDeVista = GestorDeVistaMvc.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            var vista = gestorDeVista.LeerVistaMvc($"{descriptor.Controlador}.{descriptor.Vista}");

            descriptor.Creador.AbrirEnModal = vista.MostrarEnModal;
            descriptor.Editor.AbrirEnModal = vista.MostrarEnModal;

            ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
            descriptor.GestorDeUsuario = GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            descriptor.UsuarioConectado = descriptor.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login, errorSiNoHay: true, errorSiHayMasDeUno: true, aplicarJoin: false);

            var destino = $"{(descriptor.RutaBase.IsNullOrEmpty() ? "" : $"../{descriptor.RutaBase}/")}{descriptor.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
            string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();

            if (!descriptor.UsuarioConectado.EsAdministrador)
            {
                var hayPermisos = descriptor.GestorDeUsuario.TienePermisoFuncional(descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    return RenderMensaje($"Solicite permisos de acceso a {destino}");

                hayPermisos = descriptor.GestorDeUsuario.TienePermisoDeDatos(descriptor.UsuarioConectado, enumModoDeAccesoDeDatos.Consultor, descriptor.Negocio);
                if (!hayPermisos)
                    return RenderMensaje($"Solicite al menos permisos de consulta sobre los elementos de negocio {descriptor.Negocio.ToNombre()}");
            }


            descriptor.GestorDeNegocio = GestorDeNegocios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            ViewBag.DatosDeConexion = DatosDeConexion;

            return base.View(destino, descriptor);
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
            //Descriptor.Mnt.Datos.CantidadPorLeer = cantidad;
            //Descriptor.Mnt.Datos.PosicionInicial = posicion;

            var opcionesDeMapeo = new Dictionary<string, object>();
            opcionesDeMapeo.Add(ltrParametrosDto.DescargarGestionDocumental, false);

            List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
            List<ClausulaDeOrdenacion> ordenes = orden == null ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(orden);

            if (ordenes.Count == 0 && typeof(TRegistro).ImplementaNombre())
                ordenes.Add(new ClausulaDeOrdenacion() { OrdenarPor = nameof(INombre.Nombre), Modo = ModoDeOrdenancion.ascendente });

            AntesDeEjecutar_Leer(posicion, cantidad, filtros, ordenes);

            return GestorDeElementos.LeerElementos(posicion, cantidad, filtros, ordenes, opcionesDeMapeo);

        }

        protected virtual void AntesDeEjecutar_Leer(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> ordenes)
        {

        }
    }

}

