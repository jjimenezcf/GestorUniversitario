using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using UniversidadDeMurcia.Datos;
using Microsoft.EntityFrameworkCore;
using UniversidadDeMurcia.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversidadDeMurcia.Controllers
{
    internal class Mantenimiento
    {
        private readonly string _NombreDelObjeto;

        public string Vista => $"Mnt{_NombreDelObjeto}s";
        public string Titulo => $"Mantenimiento de {_NombreDelObjeto}s";
        public string Ir => $"IraMnt{_NombreDelObjeto}s";

        public Mantenimiento(string objeto)
        {
            _NombreDelObjeto = objeto;
        }
    }

    internal class Creacion
    {
        private readonly string _NombreDelObjeto;

        public string Vista => $"Crear{_NombreDelObjeto}";
        public string Titulo => $"Creación de {_NombreDelObjeto}";
        public string Ir => $"IraCrear{_NombreDelObjeto}";

        public Creacion(string objeto)
        {
            _NombreDelObjeto = objeto;
        }
    }


    internal class Edicion
    {
        private readonly string _NombreDelObjeto;

        public string Vista => $"Editar{_NombreDelObjeto}";
        public string Titulo => $"Edición de {_NombreDelObjeto}";
        public string Ir => $"IraEditar{_NombreDelObjeto}";

        public Edicion(string objeto)
        {
            _NombreDelObjeto = objeto;
        }
    }


    internal class Detalle
    {
        private readonly string _NombreDelObjeto;

        public string Vista => $"Detalle{_NombreDelObjeto}";
        public string Titulo => $"Detalle de {_NombreDelObjeto}";
        public string Ir => $"IraDetalle{_NombreDelObjeto}";

        public Detalle(string objeto)
        {
            _NombreDelObjeto = objeto;
        }
    }

    internal class Borrado
    {
        private readonly string _NombreDelObjeto;

        public string Vista => $"Borrar{_NombreDelObjeto}";
        public string Titulo => $"Borrado de {_NombreDelObjeto}";
        public string Ir => $"IraBorrar{_NombreDelObjeto}";

        public Borrado(string objeto)
        {
            _NombreDelObjeto = objeto;
        }
    }

    public class CrudBag
    {
        internal string CrearObjeto => $"Crear{NombreDelObjeto}";

        public string NombreDelObjeto {get;}

        private Mantenimiento _Mantenimiento { get; }
        private Creacion _Creacion { get; }
        private Edicion _Edicion { get; }
        private Detalle _Detalle { get; }
        private Borrado _Borrado { get; }

        public string VistaDelCrud => _Mantenimiento.Vista;
        public string TituloDelCrud => _Mantenimiento.Titulo;
        public string IrAlCrud => _Mantenimiento.Ir;

        public string VistaDeCreacion => _Creacion.Vista;
        public string TituloDeLaCreacion => _Creacion.Titulo;
        public string IraCrear => _Creacion.Ir;

        public string VistaDeEdicion => _Edicion.Vista;
        public string TituloDeLaEdicion => _Edicion.Titulo;
        public string IraEditar => _Edicion.Ir;

        public string VistaDeDetalle => _Detalle.Vista;
        public string TituloDelDetalle => _Detalle.Titulo;
        public string IraDetalle => _Detalle.Ir;

        public string VistaDeBorrado => _Borrado.Vista;
        public string TituloDelBorrado => _Borrado.Titulo;
        public string IraBorrar => _Borrado.Ir;

        public CrudBag(string objeto)
        {
            NombreDelObjeto = objeto;
            _Mantenimiento = new Mantenimiento(NombreDelObjeto);
            _Creacion = new Creacion(NombreDelObjeto);
            _Edicion = new Edicion(NombreDelObjeto);
            _Detalle = new Detalle(NombreDelObjeto);
            _Borrado = new Borrado(NombreDelObjeto);
        }
    }


    public class BaseController : Controller
    {

        protected ContextoUniversitario ContextoDeBd { get; }

        protected Errores GestorErrores { get; }

        protected CrudBag CrudDelObjeto { get; }

        protected DbSet<Estudiante> ObjetoDeBd => ContextoDeBd.Estudiantes;

        public BaseController(Errores gestorErrores)
        {
            GestorErrores = gestorErrores;
        }

        public BaseController(ContextoUniversitario contexto, Errores gestorErrores, string objeto) :
            this(gestorErrores)
        {
            ContextoDeBd = contexto;
            CrudDelObjeto = new CrudBag(objeto);
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = CrudDelObjeto;
            return base.View(viewName, model);
        }
        
        protected async Task<IActionResult> CrearObjeto(Models.Objeto objeto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContextoDeBd.Add(objeto);
                    await ContextoDeBd.SaveChangesAsync();
                    return RedirectToAction(CrudDelObjeto.IrAlCrud);
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorErrores.Enviar("Error al crear un estudiante", e);
            }
            return View("CrearEstudiante", objeto);
        }

        protected async Task<IActionResult> ModificarObjeto(int id, Models.Objeto objeto)
        {
            if (id != objeto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ContextoDeBd.Update(objeto);
                    await ContextoDeBd.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExisteObjetoEnBd(objeto.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(CrudDelObjeto.IrAlCrud);
            }

            return View(CrudDelObjeto.VistaDeEdicion, objeto);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return ContextoDeBd.Objetos(CrudDelObjeto.NombreDelObjeto).Any(e => e.ID == id);
        }

    }

}

