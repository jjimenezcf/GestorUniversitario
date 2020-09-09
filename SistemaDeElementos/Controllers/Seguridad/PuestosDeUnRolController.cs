using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class PuestosDeUnRolController :  EntidadController<ContextoSe, RolesDeUnPuestoDtm, PuestosDeUnRolDto>
    {

        public PuestosDeUnRolController(GestorDePuestosDeUnRol gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePuestosDeUnRol(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPuestosDeUnRol(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(PuestoDto))
                return ((GestorDePuestosDeUnRol)GestorDeElementos).LeerPuestos(posicion, cantidad, filtro);

            if (claseElemento == nameof(RolDto))
                return ((GestorDePuestosDeUnRol)GestorDeElementos).LeerRoles(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
