using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestoresDeNegocio.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class PermisosDeUnUsuarioController : EntidadController<ContextoSe, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>
    {
         public PermisosDeUnUsuarioController(GestorDePermisosDeUnUsuario gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePermisosDeUnUsuario(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnUsuario(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(PermisoDto))
                return ((GestorDePermisosDeUnUsuario)GestorDeElementos).LeerPermisos(posicion, cantidad, filtro);

            if (claseElemento == nameof(RolDto))
                return ((GestorDePermisosDeUnUsuario)GestorDeElementos).LeerUsuarios(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}
