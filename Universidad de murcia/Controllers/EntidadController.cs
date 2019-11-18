using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using GestorUniversitario.ModeloDeClases;
using Microsoft.EntityFrameworkCore;
using GestorUniversitario.ContextosDeBd;
using System;

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
        public string TituloDetalle;
        public DetalleCrud(string titulo) :
        base("Detalle")
        {
            TituloDetalle = titulo;
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

        public MantenimientoCrud<T> Mantenimiento { get; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }

        public GestorCrud(string titulo, string tituloDetalle)
        {
            Titulo = titulo;
            Mantenimiento = new MantenimientoCrud<T>();
            Creador = new CreacionCrud<T>();
            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>(tituloDetalle);
            Supresor = new BorradoCrud<T>();
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
            GestorDelCrud = new GestorCrud<T>("Gestor de estudiantes", "Inscripciones");
        }

        public IActionResult Index()
        {
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        public override ViewResult View(string viewName, object model)
        {
            ViewBag.Crud = GestorDelCrud;
            return base.View(viewName, model);
        }

        protected async Task<IActionResult> CrearObjeto(Elemento elemeto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await GestorDeElementos.Crear(ContextoDeBd, elemeto);
                    return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorDeErrores.Enviar("Error al crear un estudiante", e);
            }
            return View((GestorDelCrud.Creador.Vista, elemeto));
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
                return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
            }

            return View(GestorDelCrud.Editor.Vista, elemento);
        }


        private bool ExisteObjetoEnBd(int id)
        {
            return ContextoDeBd.Elementos<T>().Any(e => e.Id == id);
        }

    }

}

