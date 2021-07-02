using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestoresDeNegocio.Entorno;
using GestorDeElementos;
using System.Collections.Generic;
using GestoresDeNegocio.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class PermisosDeUnUsuarioController : EntidadController<ContextoSe, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>
    {
         public PermisosDeUnUsuarioController(GestorDePermisosDeUnUsuario gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnUsuario()
        {
            return ViewCrud(new DescriptorDePermisosDeUnUsuario(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, filtros );

            if (claseElemento == nameof(RolDto))
                return GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad,  filtros );

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}
