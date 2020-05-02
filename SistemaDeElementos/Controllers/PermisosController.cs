using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{

    public class PermisosController : EntidadController<CtoSeguridad, PermisoDtm, PermisoDto>
    {
        public PermisosController(GestorDePermisos gestorDePermisos, GestorDeErrores gestorDeErrores) 
        : base
        (
         gestorDePermisos, 
         gestorDeErrores, 
         new CrudPermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        public IActionResult IraMantenimientoPermiso(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


        public IActionResult IraCrearPermiso()
        {
            return View(GestorDelCrud.Creador.Vista, new PermisoDto());
        }

        public IActionResult IraDetallePermiso(int? id)
        {
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarPermiso(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerPermiso(id));
        }

        public IActionResult IraEditarPermiso(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerPermiso(id));
        }


        [HttpPost, ActionName(nameof(CrearPermiso))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPermiso([Bind("Id,Nombre,Clase,Permiso")] PermisoDto permisoDto)
        {
            return await CrearObjeto(permisoDto);
        }



        [HttpPost, ActionName(nameof(ModificarPermiso))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarPermiso(int id, [Bind("Id,Nombre,Clase,Permiso")] PermisoDto permisoDto)
        {
            return await ModificarObjeto(id, permisoDto);
        }


        private PermisoDto LeerPermiso(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del permiso no puede ser nulo");
            }

            var permiso = (PermisoDto)GestorDeElementos.LeerElementoPorId((int)id);
            if (permiso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del permiso no se pudo localizar");
            }

            return permiso;
        }

        private PermisoDto LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del permiso no puede ser nulo");
            }

            var permiso = (PermisoDto)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (permiso == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del permiso no se pudo localizar");
            }

            return permiso;
        }

    }
}
