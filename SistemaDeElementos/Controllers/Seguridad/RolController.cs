using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using Gestor.Elementos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class RolController : EntidadController<ContextoSe, RolDtm, RolDto>
    {

        public RolController(GestorDeRoles gestor, GestorDeErrores gestorDeErrores)
         : base
         (
           gestor,
           gestorDeErrores,
           new DescriptorDeRol(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudRol()
        {
            return ViewCrud();
        }
    }
}
