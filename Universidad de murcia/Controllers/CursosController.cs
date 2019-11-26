using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Gestor.Errores;

namespace UniversidadDeMurcia.Controllers
{

    public class CursosController : EntidadController<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {

        public CursosController(GestorDeCursos gestorDeCursos, GestorDeErrores gestorDeErrores):
            base(gestorDeCursos, gestorDeErrores)
        {
            GestorDelCrud.Creador.AsignarTitulo("Crear un nuevo curso");
        }


        public IActionResult IraMantenimientoCurso(string orden)
        {
            var cursos = GestorDeElementos.LeerTodos();
            return View(GestorDelCrud.Mantenimiento.Vista, cursos.ToList());
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

            var curso = (ElementoCurso)GestorDeElementos.LeerElementoPorId((int) id);
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
