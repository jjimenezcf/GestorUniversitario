using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Universitario.ModeloIu;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario;
using Gestor.Errores;
using UniversidadDeMurcia.Descriptores;

namespace UniversidadDeMurcia.Controllers
{
    public class EstudiantesController : EntidadController<ContextoUniversitario, Usuario, UsuarioDto>
    {

        public EstudiantesController(GestorDeEstudiantes gestorDeEstudiantes, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeEstudiantes, 
          gestorDeErrores, 
          new CrudEstudiante(ModoDescriptor.Mantenimiento)
        )
        {
        }

        
        public IActionResult IraMantenimientoEstudiante(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden));
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }

        
        public IActionResult IraCrearEstudiante()
        {
            return View(GestorDelCrud.Creador.Vista, new UsuarioDto());
        }

        public IActionResult IraDetalleEstudiante(int? id)
        {
            GestorDelCrud.Detalle.AsignarTituloDetalle("Inscripciones");
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarEstudiante(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerEstudiante(id));
        }

        public IActionResult IraEditarEstudiante(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerEstudiante(id));
        }


        [HttpPost, ActionName(nameof(CrearEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] UsuarioDto estudiante)
        {
            return await CrearObjeto(estudiante);
        }



        [HttpPost, ActionName(nameof(ModificarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("Id,Apellido,Nombre,InscritoEl")] UsuarioDto estudiante)
        {
            return await ModificarObjeto(id, estudiante);
        }



        [HttpPost, ActionName(nameof(BorrarEstudiante))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarEstudiante(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return IraMantenimientoEstudiante("");
        }

        private UsuarioDto LeerEstudiante(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (UsuarioDto)GestorDeElementos.LeerElementoPorId((int)id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

        private UsuarioDto LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (UsuarioDto)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }


    }

}
