using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.Seguridad;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Controllers
{
    public class ClaseDePermisoController : EntidadController<ContextoSe, ClasePermisoDtm, ClasePermisoDto>
    {
        public ClaseDePermisoController(GestorDeClaseDePermisos gestor, GestorDeErrores errores)
        : base 
        (
         gestor,
         errores,
         new DescriptorDeClaseDePermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        public IActionResult CrudClaseDePermiso()
        {
            return ViewCrud();
        }


    }
}
