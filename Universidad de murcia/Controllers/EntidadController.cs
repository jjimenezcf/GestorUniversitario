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
using Extensiones.String;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversidadDeMurcia.Controllers
{
        
    public class EntidadController<TContexto, TRegistro, TElemento> : BaseController 
        where TContexto: ContextoDeElementos  
        where TRegistro : RegistroBase 
        where TElemento : ElementoBase
    {

        protected GestorDeElementos<TContexto, TRegistro,TElemento> GestorDeElementos { get; }
        protected GestorCrud<TElemento> GestorDelCrud { get; }

        public EntidadController(GestorDeElementos<TContexto, TRegistro,TElemento> gestorDeElementos, GestorDeErrores gestorErrores) :
        base(gestorErrores)
        {
            GestorDeElementos = gestorDeElementos;
            GestorDeElementos.AsignarGestores(gestorErrores);
            GestorDelCrud = new GestorCrud<TElemento>(DefinirColumnasDelGrid, DefinirOpcionesGenerales);
            DatosDeConexion = GestorDeElementos.Contexto.DatosDeConexion;
        }

        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        public JsonResult Leer(string posicion, string cantidad, string orden)
        {
            var resultado = LeerOrdenados(posicion.Entero(), cantidad.Entero(), orden);
            return new JsonResult(resultado.elementos);
        }

        public string LeerSiguientes(string posicion, string cantidad, string orden)
        {
            var resultado = LeerOrdenados(posicion.Entero(), cantidad.Entero(), orden);
            GestorDelCrud.Mantenimiento.TotalEnBd = resultado.totalEnBd;
            GestorDelCrud.Mantenimiento.FilasDelGrid = MapearElementosAlGrid(resultado.elementos);
            return GestorDelCrud.Mantenimiento.RenderGridSiguiente();
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

        protected virtual List<Opcion> DefinirOpcionesGenerales()
        {
            return new List<Opcion>();
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
                    return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
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
                return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
            }

            return View(GestorDelCrud.Editor.Vista, elemento);
        }
        
        protected (IEnumerable<TElemento> elementos, int totalEnBd) LeerOrdenados(int posicion, int cantidad, string orden)
        {
            var (elementos, total) = GestorDeElementos.Leer(posicion,cantidad, ParsearOrdenacion(orden));

            return (elementos, total);
        }

        private static Dictionary<string, Ordenacion> ParsearOrdenacion(string orden)
        {
            var ordenParseado = new Dictionary<string, Ordenacion>();

            if (!orden.IsNullOrEmpty())
            {
                var ordenes = orden.Split(';');
                var i = 0;
                while (i < ordenes.Length)
                {
                    if (ordenes[i].IsNullOrEmpty())
                        break;
                    else
                    {
                        if (i + 1 == ordenes.Length && !ordenes[i].IsNullOrEmpty())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1].IsNullOrEmpty())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1] == Ordenacion.Ascendente.ToString())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1] == Ordenacion.Descendente.ToString())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Descendente;
                            break;
                        }

                        i = i + 2;
                    }

                }
            }

            return ordenParseado;
        }

    }

}

