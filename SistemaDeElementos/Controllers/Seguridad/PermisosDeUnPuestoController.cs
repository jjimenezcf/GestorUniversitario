using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class PermisosDeUnPuestoController : EntidadController<ContextoSe, PermisosDeUnPuestoDtm, PermisosDeUnPuestoDto>
    {
         public PermisosDeUnPuestoController(GestorDePermisosDeUnPuesto gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePermisosDeUnPuesto(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnPuesto(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(PermisoDto))
                return ((GestorDePermisosDeUnPuesto)GestorDeElementos).LeerPermisos(posicion, cantidad, filtro);

            if (claseElemento == nameof(RolDto))
                return ((GestorDePermisosDeUnPuesto)GestorDeElementos).LeerPuestos(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}
