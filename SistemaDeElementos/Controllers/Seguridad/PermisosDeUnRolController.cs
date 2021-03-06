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
    public class PermisosDeUnRolController : RelacionController<ContextoSe, PermisosDeUnRolDtm, PermisosDeUnRolDto>
    {
         public PermisosDeUnRolController(GestorDePermisosDeUnRol gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPermisosDeUnRol()
        {
            return ViewCrud(new DescriptorDePermisosDeUnRol(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, filtros);

            if (claseElemento == nameof(RolDto))
                GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}
