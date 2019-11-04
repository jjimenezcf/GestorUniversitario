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
    internal class Mantenimiento<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Mnt{NombreDelObjeto}s";
        public string Titulo => $"Mantenimiento de {NombreDelObjeto}s";
        public string Ir => $"IraMnt{NombreDelObjeto}s";

        public Mantenimiento()
        {
            
        }
    }

    internal class Creacion<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Crear{NombreDelObjeto}";
        public string Titulo => $"Creación de {NombreDelObjeto}";
        public string Ir => $"IraCrear{NombreDelObjeto}";

        public Creacion()
        {
        }
    }


    internal class Edicion<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Editar{NombreDelObjeto}";
        public string Titulo => $"Edición de {NombreDelObjeto}";
        public string Ir => $"IraEditar{NombreDelObjeto}";

        public Edicion()
        {
        }
    }


    internal class Detalle<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Detalle{NombreDelObjeto}";
        public string Titulo => $"Detalle de {NombreDelObjeto}";
        public string Ir => $"IraDetalle{NombreDelObjeto}";

        public Detalle()
        { 
        }
    }

    internal class Borrado<T>
    {
        private string NombreDelObjeto => typeof(T).Name;

        public string Vista => $"Borrar{NombreDelObjeto}";
        public string Titulo => $"Borrado de {NombreDelObjeto}";
        public string Ir => $"IraBorrar{NombreDelObjeto}";

        public Borrado()
        {
        }
    }

    public class CrudBag<T>
    {
        internal string CrearObjeto => $"Crear{NombreDelObjeto}";

        public string NombreDelObjeto => typeof(T).Name;

        private Mantenimiento<T> _Mantenimiento { get; }
        private Creacion<T> _Creacion { get; }
        private Edicion<T> _Edicion { get; }
        private Detalle<T> _Detalle { get; }
        private Borrado<T> _Borrado { get; }

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

        public CrudBag()
        {
            _Mantenimiento = new Mantenimiento<T>();
            _Creacion = new Creacion<T>();
            _Edicion = new Edicion<T>();
            _Detalle = new Detalle<T>();
            _Borrado = new Borrado<T>();
        }
    }


    public class EntidadController<T> : BaseController where T: Elemento
    {

        protected ContextoUniversitario ContextoDeBd { get; }


        protected CrudBag<T> CrudDelObjeto { get; }

        protected DbSet<Estudiante> ObjetoDeBd => ContextoDeBd.Estudiantes;


        public EntidadController(ContextoUniversitario contexto, Errores gestorErrores) :
            base(gestorErrores)
        {
            ContextoDeBd = contexto;
            CrudDelObjeto = new CrudBag<T>();
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = CrudDelObjeto;
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
                return RedirectToAction(CrudDelObjeto.IrAlCrud);
            }

            return View(CrudDelObjeto.VistaDeEdicion, elemento);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return ContextoDeBd.Elementos<T>().Any(e => e.Id == id);
        }

    }

}

