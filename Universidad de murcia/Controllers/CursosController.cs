using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Gestor.Errores;
using Componentes;
using UtilidadesParaIu;
using System.Collections.Generic;
using Utilidades;
using UniversidadDeMurcia.Descriptores;

namespace UniversidadDeMurcia.Controllers
{

    public class CursosController : EntidadController<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {
        public CursosController(GestorDeCursos gestorDeCursos, GestorDeErrores gestorDeErrores) :
            base(nameof(CursosController), gestorDeCursos, gestorDeErrores, new CrudCurso())
        {
            GestorDelCrud.Creador.AsignarTitulo("Crear un nuevo curso");
            //GestorDelCrud.Modales[nameof(SelectorDeEstudiante)] = new SelectorDeEstudiante(gestorDeCursos.Contexto, gestorDeCursos.Mapeador).Selector;
        }

        public IActionResult IraMantenimientoCurso(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden));
            return ViewCrud();
        }

        private IEnumerable<ElementoCurso> OrdenarListaDeCursos(IEnumerable<ElementoCurso> cursos, string orden)
        {
            foreach (var columna in GestorDelCrud.Mantenimiento.ColumnasDelGrid)
            {
                if (!columna.Ordenar)
                    continue;

                if (orden != null && orden.Contains(columna.Nombre) && columna.Nombre == nameof(ElementoCurso.Titulo))
                {
                    if (orden.EndsWith("Des"))
                    {
                        columna.Sentido = "Asc";
                        return cursos.OrderByDescending(c=>c.Titulo);
                    }
                    columna.Sentido = "Des";
                    return cursos.OrderBy(c => c.Titulo);
                }
            }
            return cursos.OrderBy(s => s.Titulo);
        }

        private void PrepararProximoOrden(string orden)
        {
            ViewData[nameof(ElementoCurso.Titulo)] = orden.IsNullOrEmpty() || orden == $"{nameof(ElementoCurso.Titulo)}Asc"
                                                         ? $"{nameof(ElementoCurso.Titulo)}Des"
                                                         : $"{nameof(ElementoCurso.Titulo)}Asc";
        }

        public IActionResult IraCrearCurso()
        {
            return View(GestorDelCrud.Creador.Vista, new ElementoCurso());
        }

        public IActionResult IraDetalleCurso(int? id)
        {
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarCurso(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerCurso(id));
        }

        public IActionResult IraEditarCurso(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerCurso(id));
        }


        [HttpPost, ActionName(nameof(CrearCurso))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCurso([Bind("Id,Titulo,Creditos")] ElementoCurso curso)
        {
            return await CrearObjeto(curso);
        }



        [HttpPost, ActionName(nameof(ModificarCurso))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarCurso(int id, [Bind("Id,Titulo,Creditos")] ElementoCurso curso)
        {
            return await ModificarObjeto(id, curso);
        }



        [HttpPost, ActionName(nameof(BorrarCurso))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarCurso(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        private ElementoCurso LeerCurso(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del curso no puede ser nulo");
            }

            var curso = (ElementoCurso)GestorDeElementos.LeerElementoPorId((int)id);
            if (curso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del curso no se pudo localizar");
            }

            return curso;
        }

        private ElementoCurso LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del curso no puede ser nulo");
            }

            var curso = (ElementoCurso)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (curso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del curso no se pudo localizar");
            }

            return curso;
        }

    }
}
