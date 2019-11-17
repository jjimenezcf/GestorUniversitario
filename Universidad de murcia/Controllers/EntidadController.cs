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
    public class BaseCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name;
        private string _verbo;
        private string _accion;

        public string Vista => $"{_verbo}{NombreDelObjeto}";
        public string Titulo { get; set; }
        public string Ir => $"Ira{_verbo}{NombreDelObjeto}";

        public string Accion => _accion ?? $"{_verbo}{NombreDelObjeto}";

        public string Formulario => _verbo;

        public BaseCrud(string verbo)
        {
            _verbo = verbo;
        }

        public void AsignarTitulo(string titulo)
        {
            Titulo = titulo;
        }

        public void AsignarAccion(string accion)
        {
            _accion = accion;
        }

    }

    public class MantenimientoCrud<T> : BaseCrud<T>
    {
        public MantenimientoCrud() :
        base("Mantenimiento")
        {
            AsignarTitulo($"Mantenimiento de {NombreDelObjeto}s");
        }
    }

    public class CreacionCrud<T> : BaseCrud<T>
    {
        public CreacionCrud() :
        base("Crear")
        {
            AsignarTitulo($"Creación de {NombreDelObjeto}");
        }
    }


    public class EdicionCrud<T> : BaseCrud<T>
    {
        public EdicionCrud() :
        base("Editar")
        {
            AsignarTitulo($"Edición de {NombreDelObjeto}");
            AsignarAccion($"Modificar{NombreDelObjeto}");
        }
    }


    public class DetalleCrud<T> : BaseCrud<T>
    {
        public DetalleCrud() :
        base("Detalle")
        {
            AsignarTitulo($"Detalle de {NombreDelObjeto}");
        }
    }

    public class BorradoCrud<T> : BaseCrud<T>
    {
        public BorradoCrud() :
        base("Borrar")
        {
            AsignarTitulo($"Borrado de {NombreDelObjeto}");
        }
    }

    public class GestorCrud<T>
    {
        public string NombreDelObjeto => typeof(T).Name;
        public string Titulo {get;set;}

        public MantenimientoCrud<T> GestorDeMantenimiento { get; }
        public CreacionCrud<T> GestorDeCreacion { get; }
        public EdicionCrud<T> GestorDeEdicion { get; }
        public DetalleCrud<T> GestorDeDetalle { get; }
        public BorradoCrud<T> GestorDeBorrado { get; }

        public GestorCrud(string titulo)
        {
            Titulo = titulo;
            GestorDeMantenimiento = new MantenimientoCrud<T>();
            GestorDeCreacion = new CreacionCrud<T>();
            GestorDeEdicion = new EdicionCrud<T>();
            GestorDeDetalle = new DetalleCrud<T>();
            GestorDeBorrado = new BorradoCrud<T>();
        }
    }


    public class EntidadController<T> : BaseController where T : Elemento
    {

        protected ContextoUniversitario ContextoDeBd { get; }


        protected GestorCrud<T> GestorDelCrud { get; }


        public EntidadController(ContextoUniversitario contexto, Errores gestorErrores) :
        base(gestorErrores)
        {
            ContextoDeBd = contexto;
            GestorDelCrud = new GestorCrud<T>("Gestor de estudiantes");
        }

        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.GestorDeMantenimiento.Ir);
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
                    return RedirectToAction(GestorDelCrud.GestorDeMantenimiento.Ir);
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorDeErrores.Enviar("Error al crear un estudiante", e);
            }
            return View((GestorDelCrud.GestorDeCreacion.Vista, objeto));
        }

        protected async Task<IActionResult> ModificarObjeto(int id, Elemento elemento)
        {
            if (id != elemento.Id)
            {
                ModelState.AddModelError("", $"El registro pedido no se ha localizado."); ;
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
                return RedirectToAction(GestorDelCrud.GestorDeMantenimiento.Ir);
            }

            return View(GestorDelCrud.GestorDeEdicion.Vista, elemento);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return ContextoDeBd.Elementos<T>().Any(e => e.Id == id);
        }

    }

}

