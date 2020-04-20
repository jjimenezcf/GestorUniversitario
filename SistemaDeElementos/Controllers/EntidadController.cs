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

        //Llamada desde vista MVC
        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.Descriptor.VistaMnt.Ir);
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
            catch(Exception e)
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
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                r.Datos = GestorDeElementos.LeerElementoPorId(listaIds[0]);
                r.Estado = EstadoPeticion.Ok;
                r.Mensaje = $"se han leido 1 {(1>1? "registros" : "registro")}";
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

                GestorDelCrud.Descriptor.MapearElementosAlGrid(elementos);
                r.Html = GestorDelCrud.Descriptor.Mnt.Grid.RenderDelGrid(DescriptorDeCrud<TElemento>.ParsearModo(modo));
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

        //Lamada desde Selector.js
        public JsonResult Leer(string filtro)
        {
           IEnumerable<TElemento> elementos = Leer(0, -1, filtro, null);
           return new JsonResult(elementos);
        }

        public ViewResult ViewCrud()
        {
            ViewBag.DatosDeConexion = DatosDeConexion;
            return base.View(GestorDelCrud.Descriptor.VistaMnt.Vista, GestorDelCrud.Descriptor);
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = GestorDelCrud;
            ViewBag.DatosDeConexion = DatosDeConexion;
            return base.View(viewName, model);
        }


        protected async Task<IActionResult> CrearObjeto(TElemento iuElemento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await GestorDeElementos.InsertarElementoAsync(iuElemento);
                    return RedirectToAction(GestorDelCrud.Descriptor.VistaMnt.Ir);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorDeErrores.Enviar("Error al crear un usuario", e);
            }
            return View((GestorDelCrud.Creador.Vista, iuElemento));
        }

        protected async Task<IActionResult> ModificarObjeto(int id, TElemento elemento)
        {
            if (id != elemento.Id)
            {
                ModelState.AddModelError("", $"El registro pedido no se ha localizado."); ;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await GestorDeElementos.ModificarElementoAsync(elemento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GestorDeElementos.ExisteObjetoEnBd(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(GestorDelCrud.Descriptor.VistaMnt.Ir);
            }

            return View(GestorDelCrud.Editor.Vista, elemento);
        }

        protected IEnumerable<TElemento> LeerOrdenados(string orden)
        {
            var elementos = GestorDeElementos.LeerElementos(GestorDelCrud.Descriptor.Mnt.Grid.PosicionInicial
                                                          , GestorDelCrud.Descriptor.Mnt.Grid.CantidadPorLeer
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
            GestorDelCrud.Descriptor.Mnt.Grid.CantidadPorLeer = cantidad;
            GestorDelCrud.Descriptor.Mnt.Grid.PosicionInicial = posicion;

            List<ClausulaDeFiltrado> filtros = filtro == null ? new List<ClausulaDeFiltrado>(): JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);

            return GestorDeElementos.LeerElementos(posicion, cantidad, filtros, orden.ParsearOrdenacion());
        }

    }

}

