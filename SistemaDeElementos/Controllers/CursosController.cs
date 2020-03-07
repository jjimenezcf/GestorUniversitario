using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.Permiso;

namespace MVCSistemaDeElementos.Controllers
{

    public class CursosController : EntidadController<CtoPermisos, PermisoReg, PermisoDto>
    {
        public CursosController(GestorDeCursos gestorDeCursos, GestorDeErrores gestorDeErrores) 
        : base
        (
         gestorDeCursos, 
         gestorDeErrores, 
         new CrudCurso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        public IActionResult IraMantenimientoCurso(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden));
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


        public IActionResult IraCrearCurso()
        {
            return View(GestorDelCrud.Creador.Vista, new PermisoDto());
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
        public async Task<IActionResult> CrearCurso([Bind("Id,Titulo,Creditos")] PermisoDto curso)
        {
            return await CrearObjeto(curso);
        }



        [HttpPost, ActionName(nameof(ModificarCurso))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarCurso(int id, [Bind("Id,Titulo,Creditos")] PermisoDto curso)
        {
            return await ModificarObjeto(id, curso);
        }



        [HttpPost, ActionName(nameof(BorrarCurso))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarCurso(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return IraMantenimientoCurso("");
        }

        private PermisoDto LeerCurso(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del curso no puede ser nulo");
            }

            var curso = (PermisoDto)GestorDeElementos.LeerElementoPorId((int)id);
            if (curso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del curso no se pudo localizar");
            }

            return curso;
        }

        private PermisoDto LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del curso no puede ser nulo");
            }

            var curso = (PermisoDto)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (curso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del curso no se pudo localizar");
            }

            return curso;
        }

    }
}
