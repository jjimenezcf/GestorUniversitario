using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using GestoresDeNegocio.Seguridad;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class ClaseDePermisoController : EntidadController<ContextoSe, ClasePermisoDtm, ClasePermisoDto>
    {
        public ClaseDePermisoController(GestorDeClaseDePermisos gestor, GestorDeErrores errores)
        : base 
        (
         gestor,
         errores
        )
        {
        }

        public IActionResult CrudClaseDePermiso()
        {
            return ViewCrud(new DescriptorDeClaseDePermiso(Contexto, ModoDescriptor.Mantenimiento));
        }


    }
}
