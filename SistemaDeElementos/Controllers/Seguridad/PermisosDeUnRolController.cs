using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class PermisosDeUnRolController : EntidadController<ContextoSe, PermisosDeUnRolDtm, PermisosDeUnRolDto>
    {
         public PermisosDeUnRolController(GestorDePermisosDeUnRol gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePermisosDeUnRol(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnRol(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(PermisoDto))
                return ((GestorDePermisosDeUnRol)GestorDeElementos).LeerPermisos(posicion, cantidad, filtro);

            if (claseElemento == nameof(RolDto))
                return ((GestorDePermisosDeUnRol)GestorDeElementos).LeerRoles(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}
