using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class RolesDeUnPermisoController : EntidadController<ContextoSe, PermisosDeUnRolDtm, RolesDeUnPermisoDto>
    {
        public RolesDeUnPermisoController(GestorDeRolesDeUnPermiso gestor, GestorDeErrores errores)
        : base
        (
          gestor,
          errores,
          new DescriptorDeRolesDeUnPermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        [HttpPost]
        public IActionResult CrudRolesDeUnPermiso(string restrictor, string orden)
        {
            return ViewCrud();
        }

        //protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        //{
        //    if (claseElemento == nameof(PermisoDto))
        //        return ((GestorDePermisosDeUnRol)GestorDeElementos).LeerPermisos(posicion, cantidad, filtro);

        //    if (claseElemento == nameof(RolDto))
        //        return ((GestorDePermisosDeUnRol)GestorDeElementos).LeerRoles(posicion, cantidad, filtro);

        //    return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        //}

    }
}

