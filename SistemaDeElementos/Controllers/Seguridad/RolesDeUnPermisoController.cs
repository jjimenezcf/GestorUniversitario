﻿using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class RolesDeUnPermisoController : RelacionController<ContextoSe, PermisosDeUnRolDtm, RolesDeUnPermisoDto>
    {
        public RolesDeUnPermisoController(GestorDeRolesDeUnPermiso gestor, GestorDeErrores errores)
        : base
        (
          gestor,
          errores
        )
        {
        }

        [HttpPost]
        public IActionResult CrudRolesDeUnPermiso()
        {
            return ViewCrud(new DescriptorDeRolesDeUnPermiso(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, filtros);

            if (claseElemento == nameof(RolDto))
                return GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}

