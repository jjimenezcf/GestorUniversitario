using Microsoft.AspNetCore.Mvc;
using System;
using Gestor.Errores;
using GestorDeElementos;
using UtilidadesParaIu;
using System.Collections.Generic;
using Utilidades;
using MVCSistemaDeElementos.UtilidadesIu;
using Newtonsoft.Json;
using MVCSistemaDeElementos.Descriptores;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using System.Reflection;
using ModeloDeDto;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Authorization;
using ServicioDeDatos.Entorno;

namespace MVCSistemaDeElementos.Controllers
{

    enum epAcciones { buscar, siguiente, anterior, ultima, ordenar }

    [Authorize]
    public class EntidadController<TContexto, TRegistro, TElemento> : BaseController
        where TContexto : ContextoSe
        where TRegistro : Registro
        where TElemento : ElementoDto
    {

        protected GestorDeElementos<TContexto, TRegistro, TElemento> GestorDeElementos { get; }
        protected GestorCrud<TElemento> GestorDelCrud { get; }


        public EntidadController(GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores, DescriptorDeCrud<TElemento> descriptor)
        : this(gestorDeElementos, gestorErrores)
        {
            GestorDelCrud = new GestorCrud<TElemento>(descriptor);
            var vista = ValidarExisteVista(descriptor.Controlador, descriptor.Vista);

            descriptor.Creador.AbrirEnModal = vista.MostrarEnModal;
            descriptor.Editor.AbrirEnModal = vista.MostrarEnModal;
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
            return RedirectToAction(GestorDelCrud.Descriptor.Vista);
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
            var r = new Resultado();

            try
            {
                if (fichero == null)
                    throw new Exception("No se ha identificado el fichero");

                ValidarExtension(fichero, extensionesValidas);
                var rutaBase = @"..\SistemaDeElementos\wwwroot";
                var rutaDeDescarga = $@"{rutaBase}\Archivos";
                var rutaConFichero = $@"{rutaDeDescarga}\{fichero.FileName}";

                using (var stream = new FileStream(rutaConFichero, FileMode.Create))
                {
                    fichero.CopyTo(stream);
                }

                if (rutaDestino.IsNullOrEmpty())
                {
                    r.Datos = GestoresDeNegocio.Archivos.GestorDocumental.SubirArchivo(GestorDeElementos.Contexto, rutaConFichero, GestorDeElementos.Mapeador);
                }
                else
                {
                    rutaDestino = $@"{rutaBase}{rutaDestino.Replace("/", @"\")}";

                    if (!Directory.Exists(rutaDestino))
                        Directory.CreateDirectory(rutaDestino);

                    if (!System.IO.File.Exists($@"{rutaDestino}\{fichero.FileName}"))
                        System.IO.File.Move(rutaConFichero, $@"{rutaDestino}\{fichero.FileName}");

                    r.Datos = fichero.FileName;
                }

                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = "fichero subido";
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido subir el fichero";
            }


            return new JsonResult(r);

        }

        private void ValidarExtension(IFormFile fichero, string extensiones)
        {
            if (extensiones.IsNullOrEmpty() || extensiones.EndsWith("*"))
                return;

            if (EsImagen(fichero) && (extensiones.Contains("png")
                                   || extensiones.Contains("jpg")
                                   || extensiones.Contains("svg")
                                   ))
                return;

            throw new Exception($"Para el tipo de fichero {fichero.ContentType} sólo se aceptan '{extensiones}'");

        }

        private bool EsImagen(IFormFile fichero)
        {
            return fichero.ContentType == "image/jpeg"
                || fichero.ContentType == "image/png"
                || fichero.ContentType == "image/gif"
                || fichero.ContentType == "image/jpg"
                || fichero.ContentType == "image/vnd.Microsoft.icon"
                || fichero.ContentType == "image/x-icon"
                || fichero.ContentType == "image/vnd.djvu"
                || fichero.ContentType == "image/svg+xml";
        }
        //END-POINT: Desde CrudCreacion.ts
        public JsonResult epCrearElemento(string elementoJson)
        {
            var r = new Resultado();

            try
            {
                var elemento = JsonConvert.DeserializeObject<TElemento>(elementoJson);
                GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(TipoOperacion.Insertar));
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = "Registro creado";
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido crear";
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde CrudEdicion.ts
        public JsonResult epModificarPorId(string elementoJson)
        {
            var r = new Resultado();

            try
            {
                var elemento = JsonConvert.DeserializeObject<TElemento>(elementoJson);
                GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(TipoOperacion.Modificar));
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = "Registro modificado";
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido modificar";
            }

            return new JsonResult(r);
        }


        //END-POINT: Desde CrudEdicion.ts
        public JsonResult epLeerPorIds(string idsJson)
        {
            var r = new Resultado();

            try
            {
                var elementos = Leer(0, -1, idsJson, null).ToList();

                if (elementos.Count == 0)
                    throw new Exception($"No se ha localizado el registro con el filtro {idsJson}");

                if (elementos.Count > 1)
                    throw new Exception($"Hay más de un registro para el filtro {idsJson}");

                r.Datos = elementos;
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = $"se han leido 1 {(1 > 1 ? "registros" : "registro")}";
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "Error al leer";
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
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                foreach (var id in listaIds)
                {
                    var elemento = GestorDeElementos.LeerElementoPorId(id);
                    GestorDeElementos.PersistirElementoDto(elemento, new ParametrosDeNegocio(TipoOperacion.Eliminar));
                }
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = listaIds.Count > 1 ? "Registros eliminados" : "Registro eliminado";
                GestorDeElementos.Commit(tran);
            }
            catch (Exception e)
            {
                GestorDeElementos.Rollback(tran);
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = GestorDeErrores.Mostrar(excepcion: e) ? e.Message : "No se ha podido eliminar";
            }

            return new JsonResult(r);
        }

        public class ResultadoDeLectura
        {
            public List<Dictionary<string, object>> registros { get; set; }
            public int total { get; set; }
        }

        //END-POINT: Desde GridDeDatos.ts
        public JsonResult epLeerDatosParaElGrid(string modo, string accion, string posicion, string cantidad, string filtro, string orden)
        {
            var r = new Resultado();
            int pos = posicion.Entero();
            int can = cantidad.Entero();
            try
            {
                var elementos = Leer(pos, can, filtro, orden);
                //si no he leido nada por estar al final, vuelvo a leer los últimos
                if (pos > 0 && elementos.Count() == 0)
                {
                    pos = pos - can;
                    if (pos < 0) pos = 0;
                    elementos = Leer(pos, can, filtro, orden);
                    r.Mensaje = "No hay más elementos";
                }

                var infoObtenida = new ResultadoDeLectura();

                infoObtenida.registros = ElementosLeidos(elementos.ToList());
                infoObtenida.total = accion == epAcciones.buscar.ToString() ? Contar(filtro) : Recontar(filtro);

                r.Datos = infoObtenida;
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido recuperar datos para el grid";
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
                    listaDeElementos.Add(registro);
                }
            }

            return listaDeElementos;
        }


        //END-POINT: Desde ModalSeleccion.ts
        public JsonResult epLeer(string filtro)
        {
            var r = new Resultado();
            List<TElemento> elementos;
            try
            {
                elementos = Leer(0, -1, filtro, null).ToList();
                r.Datos = elementos;
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido leer los datos";
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
                elementos = CargarLista(claseElemento);
                r.Datos = elementos;
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido leer los datos";
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
                elementos = CargaDinamica(claseElemento, posicion, cantidad, filtro);
                r.Datos = elementos;
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido leer los datos";
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
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "Error en el proceso de relación";
            }

            return new JsonResult(r);
        }


        protected virtual dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            throw new Exception($"Debe implementar la función de CargaDinamica para la clase '{claseElemento}' en el controlador '{this.GetType().Name}'");
        }

        protected virtual dynamic CargarLista(string claseElemento)
        {
            throw new NotImplementedException();
        }

        public ViewResult ViewCrud()
        {

            DatosDeConexion.Login = ObtenerUsuarioDeLaRequest();
            var destino = $"{(GestorDelCrud.Descriptor.RutaVista.IsNullOrEmpty() ? "" : $"../{GestorDelCrud.Descriptor.RutaVista}/")}{GestorDelCrud.Descriptor.Vista}";
            try
            {
                ViewBag.DatosDeConexion = DatosDeConexion;
            }
            catch (Exception e)
            {
                GestorDeErrores.Emitir($"Error al acceder a {destino}", e);
            }

            string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
            string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();
            ValidarAcceso(nombreDelControlador, nombreDeLaVista, DatosDeConexion.Login);

            return base.View(destino, GestorDelCrud.Descriptor);
        }


        private void ValidarAcceso(string nombreDelControlador, string nombreDeLaVista, string login)
        {
            var vista = ValidarExisteVista(nombreDelControlador, nombreDeLaVista);
            ValidarPermiso(vista, login);
        }

        private void ValidarPermiso(VistaMvcDtm vista, string login)
        {
            var gestorDeVista = GestorDeVistaMvc.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);
            gestorDeVista.ValidarAcceso(vista, login);

        }

        private VistaMvcDtm ValidarExisteVista(string nombreDelControlador, string nombreDeLaVista)
        {
            var cache = ServicioDeCaches.Obtener(GestorDeVistaMvc.CacheDeValidarVista);
            if (!cache.ContainsKey($"{nombreDelControlador}.{nombreDeLaVista}"))
            {

                var gestorDeVista = GestorDeVistaMvc.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador);

                var vista = gestorDeVista.LeerVistaMvc($"{nombreDelControlador}.{nombreDeLaVista}");
                if (vista == null)
                    GestorDeErrores.Emitir($"Defina la vista {nombreDelControlador}.{nombreDeLaVista} en BD");

                cache[$"{nombreDelControlador}.{nombreDeLaVista}"] = vista;
            }

            return (VistaMvcDtm)cache[$"{nombreDelControlador}.{nombreDeLaVista}"];
        }

        public ViewResult ViewCrud<T>(DescriptorDeCrud<T> descriptor)
        where T : ElementoDto
        {
            ViewBag.DatosDeConexion = DatosDeConexion;
            return base.View(descriptor.Vista, descriptor);
        }


        protected IEnumerable<TElemento> LeerOrdenados(string filtro, string orden)
        {

            List<ClausulaDeFiltrado> filtros = filtro.IsNullOrEmpty()
                                               ? new List<ClausulaDeFiltrado>()
                                               : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);


            var elementos = GestorDeElementos.LeerElementos(GestorDelCrud.Descriptor.Mnt.Datos.PosicionInicial
                                                          , GestorDelCrud.Descriptor.Mnt.Datos.CantidadPorLeer
                                                          , filtros
                                                          , orden.ParsearOrdenacion());

            return elementos;
        }

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
            GestorDelCrud.Descriptor.Mnt.Datos.CantidadPorLeer = cantidad;
            GestorDelCrud.Descriptor.Mnt.Datos.PosicionInicial = posicion;

            List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
            List<ClausulaDeOrdenacion> ordenes = orden == null ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(orden);

            return GestorDeElementos.LeerElementos(posicion, cantidad, filtros, ordenes);
        }


        //END-POINT: Desde ModalSeleccion.ts
        //public JsonResult epRecargarModalEnHtml(string idModal, string posicion, string cantidad, string filtro, string orden)
        //{
        //    var r = new ResultadoHtml();
        //    int pos = posicion.Entero();
        //    int can = cantidad.Entero();
        //    try
        //    {
        //        //si me pide leer los últimos registros
        //        if (pos == -1)
        //        {
        //            var total = Contar();
        //            pos = total - can;
        //            if (pos < 0) pos = 0;
        //            posicion = pos.ToString();
        //        }

        //        var elementos = Leer(pos, can, filtro, orden);
        //        //si no he leido nada por estar al final, vuelvo a leer los últimos
        //        if (pos > 0 && elementos.ToList().Count() == 0)
        //        {
        //            pos = pos - can;
        //            if (pos < 0) pos = 0;
        //            elementos = Leer(pos, can, filtro, orden);
        //            r.Mensaje = "No hay más elementos";
        //        }

        //        GestorDelCrud.Descriptor.MapearElementosAlGrid(elementos, can, pos);
        //        r.Html = GestorDelCrud.Descriptor.Mnt.Datos.RenderDelGridModal(idModal);
        //        r.Datos = elementos.Count();
        //        r.Estado = EstadoPeticion.Ok;
        //    }
        //    catch (Exception e)
        //    {
        //        r.Estado = EstadoPeticion.Error;
        //        r.consola = GestorDeErrores.Concatenar(e);
        //        r.Mensaje = "No se ha podido recuperar datos para el grid";
        //    }

        //    return new JsonResult(r);
        //}


        //END-POINT: Desde CrudMantenimiento.ts (Obsoleto)
        //public JsonResult epLeerGridHtml(string modo, string posicion, string cantidad, string filtro, string orden)
        //{
        //    var r = new ResultadoHtml();
        //    int pos = posicion.Entero();
        //    int can = cantidad.Entero();
        //    try
        //    {
        //        //si me pide leer los últimos registros
        //        if (pos == -1)
        //        {
        //            var total = Contar();
        //            pos = total - can;
        //            if (pos < 0) pos = 0;
        //            posicion = pos.ToString();
        //        }

        //        var elementos = Leer(pos, can, filtro, orden);
        //        //si no he leido nada por estar al final, vuelvo a leer los últimos
        //        if (pos > 0 && elementos.ToList().Count() == 0)
        //        {
        //            pos -= can;
        //            if (pos < 0) pos = 0;
        //            elementos = Leer(pos, can, filtro, orden);
        //            r.Mensaje = "No hay más elementos";
        //        }

        //        GestorDelCrud.Descriptor.MapearElementosAlGrid(elementos, can, pos);
        //        r.Html = GestorDelCrud.Descriptor.Mnt.Datos.RenderDelGrid(DescriptorDeCrud<TElemento>.ParsearModo(modo));
        //        r.Datos = elementos.Count();
        //        r.Estado = EstadoPeticion.Ok;
        //    }
        //    catch (Exception e)
        //    {
        //        r.Estado = EstadoPeticion.Error;
        //        r.consola = GestorDeErrores.Concatenar(e);
        //        r.Mensaje = "No se ha podido recuperar datos para el grid";
        //    }

        //    return new JsonResult(r);
        //}


    }

}

