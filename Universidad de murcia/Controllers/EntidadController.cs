using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using GestorUniversitario.ModeloDeClases;
using Microsoft.EntityFrameworkCore;
using GestorUniversitario.ContextosDeBd;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversidadDeMurcia.Controllers
{
    internal class MantenimientoCrud<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Mnt{NombreDelObjeto}s";
        public string Titulo => $"Mantenimiento de {NombreDelObjeto}s";
        public string Ir => $"IraMnt{NombreDelObjeto}s";

        public MantenimientoCrud()
        {
            
        }
    }

    internal class CreacionCrud<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Crear{NombreDelObjeto}";
        public string Titulo => $"Creación de {NombreDelObjeto}";
        public string Ir => $"IraCrear{NombreDelObjeto}";

        public CreacionCrud()
        {
        }
    }


    internal class EdicionCrud<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Editar{NombreDelObjeto}";
        public string Titulo => $"Edición de {NombreDelObjeto}";
        public string Ir => $"IraEditar{NombreDelObjeto}";

        public EdicionCrud()
        {
        }
    }


    internal class DetalleCrud<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Detalle{NombreDelObjeto}";
        public string Titulo => $"Detalle de {NombreDelObjeto}";
        public string Ir => $"IraDetalle{NombreDelObjeto}";

        public DetalleCrud()
        { 
        }
    }

    internal class BorradoCrud<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Borrar{NombreDelObjeto}";
        public string Titulo => $"Borrado de {NombreDelObjeto}";
        public string Ir => $"IraBorrar{NombreDelObjeto}";

        public BorradoCrud()
        {
        }
    }

    public class GestorCrud<T>
    {
        internal string CrearObjeto => $"Crear{NombreDelObjeto}";

        public string NombreDelObjeto => typeof(T).Name;

        private MantenimientoCrud<T> _Mantenimiento { get; }
        private CreacionCrud<T> _Creacion { get; }
        private EdicionCrud<T> _Edicion { get; }
        private DetalleCrud<T> _Detalle { get; }
        private BorradoCrud<T> _Borrado { get; }

        public string VistaDelCrud => _Mantenimiento.Vista;
        public string TituloDelCrud => _Mantenimiento.Titulo;
        public string IrAlMantenimiento => _Mantenimiento.Ir;

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

        public GestorCrud()
        {
            _Mantenimiento = new MantenimientoCrud<T>();
            _Creacion = new CreacionCrud<T>();
            _Edicion = new EdicionCrud<T>();
            _Detalle = new DetalleCrud<T>();
            _Borrado = new BorradoCrud<T>();
        }
    }


    public class EntidadController<T> : BaseController where T: Elemento
    {

        protected ContextoUniversitario ContextoDeBd { get; }


        protected GestorCrud<T> GestorDelCrud { get; }

        
        public EntidadController(ContextoUniversitario contexto, Errores gestorErrores) :
            base(gestorErrores)
        {
            ContextoDeBd = contexto;
            GestorDelCrud = new GestorCrud<T>();
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = GestorDelCrud;
            return base.View(viewName, model);
        }
        
        protected async Task<IActionResult> CrearObjeto(Elemento objeto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContextoDeBd.Add(objeto);
                    await ContextoDeBd.SaveChangesAsync();
                    return RedirectToAction(GestorDelCrud.IrAlMantenimiento);
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorDeErrores.Enviar("Error al crear un estudiante", e);
            }
            return View((GestorDelCrud.VistaDeCreacion, objeto));
        }

        protected async Task<IActionResult> ModificarObjeto(int id, Elemento elemento)
        {
            if (id != elemento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ContextoDeBd.Update(elemento);
                    await ContextoDeBd.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExisteObjetoEnBd(elemento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(GestorDelCrud.IrAlMantenimiento);
            }

            return View(GestorDelCrud.VistaDeEdicion, elemento);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return ContextoDeBd.Elementos<T>().Any(e => e.Id == id);
        }

    }

}

