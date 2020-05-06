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

        public IActionResult CrudPermiso(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


    }
}
