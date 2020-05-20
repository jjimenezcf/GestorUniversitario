using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;
using Gestor.Elementos;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;
using System.Collections.Generic;
using Utilidades;
using MVCSistemaDeElementos.UtilidadesIu;
using Newtonsoft.Json;
using MVCSistemaDeElementos.Descriptores;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using System.IO;
using Gestor.Elementos.Entorno;
using GestorDeElementos;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Gestor.Elementos.Seguridad;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCSistemaDeElementos.Controllers
{

    public class EntidadController<TContexto, TRegistro, TElemento> : BaseController
        where TContexto : ContextoDeElementos
        where TRegistro : Registro
        where TElemento : Elemento
    {

        protected GestorDeElementos<TContexto, TRegistro, TElemento> GestorDeElementos { get; }
        protected GestorCrud<TElemento> GestorDelCrud { get; }

        public EntidadController(GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores, DescriptorDeCrud<TElemento> descriptor)
        : base(gestorErrores)
        {
            GestorDeElementos = gestorDeElementos;
            GestorDeElementos.Contexto.IniciarTraza();
            GestorDeElementos.AsignarGestores(gestorErrores);
            GestorDelCrud = new GestorCrud<TElemento>(descriptor);
            DatosDeConexion = GestorDeElementos.Contexto.DatosDeConexion;
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

        //END-POIN: desde el ApiDeArchivos
        [HttpPost]
        public JsonResult epSubirArchivo(IFormFile fichero)
        {
            var r = new Resultado();

            //var filePath = Path.GetTempFileName();

            var rutaFichero = $@".\wwwroot\Archivos\{fichero.FileName}";

            using (var stream = new FileStream(rutaFichero, FileMode.Create))
            {
                fichero.CopyTo(stream);
            }

            var contexto = CtoSeguridad.CrearContexto();

            var gestorDeVariables = (GestorDeClaseDePermisos) 
                Generador<CtoSeguridad, IMapper>.GenerarObjeto(contexto.GetType().Assembly.GetName().Name
                                                             , nameof(GestorDeClaseDePermisos)
                                                             , new object[] { contexto, GestorDeElementos.Mapeador });

            var variable = gestorDeVariables.LeerRegistros(0
                , 1
                , new List<ClausulaDeFiltrado>() { new ClausulaDeFiltrado() { Criterio = CriteriosDeFiltrado.igual, Propiedad = nameof(VariableDto.Nombre), Valor = Variable.Servidor_Archivos } }
                );


            GestorDeElementos.SubirArchivo(rutaFichero);

            return new JsonResult(r);

        }


        //END-POINT: Desde CrudCreacion.ts
        public JsonResult epCrearElemento(string elementoJson)
        {
            var r = new Resultado();

            try
            {
                var elemento = JsonConvert.DeserializeObject<TElemento>(elementoJson);
                GestorDeElementos.PersistirElemento(elemento, new ParametrosDeNegocio(TipoOperacion.Insertar));
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
                GestorDeElementos.PersistirElemento(elemento, new ParametrosDeNegocio(TipoOperacion.Modificar));
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

            try
            {
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                var elemento = GestorDeElementos.LeerElementoPorId(listaIds[0]);
                GestorDeElementos.PersistirElemento(elemento, new ParametrosDeNegocio(TipoOperacion.Eliminar));
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = "Registro eliminado";
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = GestorDeErrores.Mostrar(excepcion: e) ? e.Message : "No se ha podido eliminar";
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde CrudMantenimiento.ts
        public JsonResult epLeerGridHtml(string modo, string posicion, string cantidad, string filtro, string orden)
        {
            var r = new ResultadoHtml();
            int pos = posicion.Entero();
            int can = cantidad.Entero();
            try
            {
                //si me pide leer los últimos registros
                if (pos == -1)
                {
                    var total = Contar();
                    pos = total - can;
                    if (pos < 0) pos = 0;
                    posicion = pos.ToString();
                }

                var elementos = Leer(pos, can, filtro, orden);
                //si no he leido nada por estar al final, vuelvo a leer los últimos
                if (pos > 0 && elementos.ToList().Count() == 0)
                {
                    pos = pos - can;
                    if (pos < 0) pos = 0;
                    elementos = Leer(pos, can, filtro, orden);
                    r.Mensaje = "No hay más elementos";
                }

                GestorDelCrud.Descriptor.MapearElementosAlGrid(elementos, can, pos);
                r.Html = GestorDelCrud.Descriptor.Mnt.Datos.RenderDelGrid(DescriptorDeCrud<TElemento>.ParsearModo(modo));
                r.Datos = elementos.Count();
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido recuperar datos para el grid";
            }

            return new JsonResult(r);
        }

        //END-POINT: Desde ModalSeleccion.ts
        public JsonResult epRecargarModalEnHtml(string idModal, string posicion, string cantidad, string filtro, string orden)
        {
            var r = new ResultadoHtml();
            int pos = posicion.Entero();
            int can = cantidad.Entero();
            try
            {
                //si me pide leer los últimos registros
                if (pos == -1)
                {
                    var total = Contar();
                    pos = total - can;
                    if (pos < 0) pos = 0;
                    posicion = pos.ToString();
                }

                var elementos = Leer(pos, can, filtro, orden);
                //si no he leido nada por estar al final, vuelvo a leer los últimos
                if (pos > 0 && elementos.ToList().Count() == 0)
                {
                    pos = pos - can;
                    if (pos < 0) pos = 0;
                    elementos = Leer(pos, can, filtro, orden);
                    r.Mensaje = "No hay más elementos";
                }

                GestorDelCrud.Descriptor.MapearElementosAlGrid(elementos, can, pos);
                r.Html = GestorDelCrud.Descriptor.Mnt.Datos.RenderDelGridModal(idModal);
                r.Datos = elementos.Count();
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido recuperar datos para el grid";
            }

            return new JsonResult(r);
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

        //END-POINT: Desde CrudMantenimiento.ts
        public JsonResult epLeerTodos(string claseElemento)
        {
            var r = new Resultado();
            dynamic elementos;
            try
            {
                elementos = LeerTodos(claseElemento);
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

        protected virtual dynamic LeerTodos(string claseElemento)
        {
            throw new NotImplementedException();
        }

        public ViewResult ViewCrud()
        {
            ViewBag.DatosDeConexion = DatosDeConexion;
            return base.View(GestorDelCrud.Descriptor.Vista, GestorDelCrud.Descriptor);
        }


        protected IEnumerable<TElemento> LeerOrdenados(string orden)
        {
            var elementos = GestorDeElementos.LeerElementos(GestorDelCrud.Descriptor.Mnt.Datos.PosicionInicial
                                                          , GestorDelCrud.Descriptor.Mnt.Datos.CantidadPorLeer
                                                          , new List<ClausulaDeFiltrado>()
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

        protected IEnumerable<TElemento> Leer(int posicion, int cantidad, string filtro, string orden)
        {
            GestorDelCrud.Descriptor.Mnt.Datos.CantidadPorLeer = cantidad;
            GestorDelCrud.Descriptor.Mnt.Datos.PosicionInicial = posicion;

            List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
            List<ClausulaDeOrdenacion> ordenes = orden == null ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(orden);

            return GestorDeElementos.LeerElementos(posicion, cantidad, filtros, ordenes);
        }

    }

}

