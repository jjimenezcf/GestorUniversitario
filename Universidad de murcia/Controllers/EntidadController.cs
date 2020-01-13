﻿using System.Threading.Tasks;
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
using UniversidadDeMurcia.UtilidadesIu;

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

        public string Leer(string idGrid, string posicion, string cantidad, string orden)
        {
            GestorDelCrud.Mantenimiento.PosicionInicial = posicion.Entero();
            GestorDelCrud.Mantenimiento.CantidadPorLeer = cantidad.Entero();
            var resultado = LeerOrdenados(orden);
            GestorDelCrud.Mantenimiento.TotalEnBd = resultado.totalEnBd;
            GestorDelCrud.Mantenimiento.FilasDelGrid = MapearElementosAlGrid(resultado.elementos);
            return GestorDelCrud.Mantenimiento.RenderGridSiguiente(idGrid);
        }

        public string LeerSiguientes(string idGrid, string posicion, string cantidad, string orden)
        {
            GestorDelCrud.Mantenimiento.PosicionInicial = posicion.Entero();
            GestorDelCrud.Mantenimiento.CantidadPorLeer = cantidad.Entero();
            var resultado = LeerOrdenados(orden);
            GestorDelCrud.Mantenimiento.TotalEnBd = resultado.totalEnBd;
            GestorDelCrud.Mantenimiento.FilasDelGrid = MapearElementosAlGrid(resultado.elementos);
            return GestorDelCrud.Mantenimiento.RenderGridSiguiente(idGrid);
        }

        public string LeerAnteriores(string idGrid, string posicion, string cantidad, string orden)
        {
            GestorDelCrud.Mantenimiento.PosicionInicial = posicion.Entero();
            GestorDelCrud.Mantenimiento.CantidadPorLeer = cantidad.Entero();
            var resultado = LeerOrdenados(orden);
            GestorDelCrud.Mantenimiento.TotalEnBd = resultado.totalEnBd;
            GestorDelCrud.Mantenimiento.FilasDelGrid = MapearElementosAlGrid(resultado.elementos);
            return GestorDelCrud.Mantenimiento.RenderGridSiguiente(idGrid);
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
        
        protected (IEnumerable<TElemento> elementos, int totalEnBd) LeerOrdenados(string orden)
        {
            var (elementos, total) = GestorDeElementos.Leer(GestorDelCrud.Mantenimiento.PosicionInicial
                                                          , GestorDelCrud.Mantenimiento.CantidadPorLeer
                                                          , Utilidades.ParsearOrdenacion(orden));

            return (elementos, total);
        }

    }

}

