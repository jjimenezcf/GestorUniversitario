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
using UniversidadDeMurcia.UtilidadesIu;
using Newtonsoft.Json;
using System.IO;
using UniversidadDeMurcia.Descriptores;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversidadDeMurcia.Controllers
{

    public class EntidadController<TContexto, TRegistro, TElemento> : BaseController
        where TContexto : ContextoDeElementos
        where TRegistro : RegistroBase
        where TElemento : ElementoBase
    {

        protected GestorDeElementos<TContexto, TRegistro, TElemento> GestorDeElementos { get; }
        protected GestorCrud<TElemento> GestorDelCrud { get; }


        public EntidadController(string controlador, GestorDeElementos<TContexto, TRegistro, TElemento> gestorDeElementos, GestorDeErrores gestorErrores, DescriptorDeCrud<TElemento> descriptor)
        : base(gestorErrores)
        {
            GestorDeElementos = gestorDeElementos;
            GestorDeElementos.Contexto.IniciarTraza();
            GestorDeElementos.AsignarGestores(gestorErrores);
            GestorDelCrud = new GestorCrud<TElemento>(controlador, DefinirColumnasDelGrid, DefinirOpcionesGenerales, descriptor);
            DatosDeConexion = GestorDeElementos.Contexto.DatosDeConexion;
        }


        protected override void Dispose(bool disposing)
        {
            GestorDeElementos.Contexto.CerrarTraza();
            base.Dispose(disposing);
        }

        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.Descriptor.VistaMnt.Ir);
        }

        public string LeerDatosDelGrid(string idGrid, string posicion, string cantidad, string filtro, string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(Leer(posicion.Entero(), cantidad.Entero(), filtro, orden));
            return GestorDelCrud.Descriptor.Grid.RenderDelGrid();
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

        protected virtual List<ColumnaDelGrid> DefinirColumnasDelGrid()
        {
            return new List<ColumnaDelGrid>();
        }

        protected virtual List<PeticionMvc> DefinirOpcionesGenerales()
        {
            return new List<PeticionMvc>();
        }

        protected virtual List<FilaDelGrid> MapearElementosAlGrid(IEnumerable<TElemento> elementos)
        {
            return new List<FilaDelGrid>();
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
                GestorDeErrores.Enviar("Error al crear un estudiante", e);
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

        protected (IEnumerable<TElemento> elementos, int totalEnBd) LeerOrdenados(string orden)
        {
            var (elementos, total) = GestorDeElementos.Leer(GestorDelCrud.Descriptor.Grid.PosicionInicial
                                                          , GestorDelCrud.Descriptor.Grid.CantidadPorLeer
                                                          , new List<FiltroSql>()
                                                          , orden.ParsearOrdenacion());

            return (elementos, total);
        }


        protected (IEnumerable<TElemento> elementos, int totalEnBd) Leer(int posicion, int cantidad, string filtro, string orden)
        {
            GestorDelCrud.Descriptor.Grid.CantidadPorLeer = cantidad;
            GestorDelCrud.Descriptor.Grid.PosicionInicial = posicion;

            List<FiltroSql> filtros = filtro == null ? new List<FiltroSql>(): JsonConvert.DeserializeObject<List<FiltroSql>>(filtro);

            return GestorDeElementos.Leer(posicion, cantidad, filtros, orden.ParsearOrdenacion());
        }

    }

}

