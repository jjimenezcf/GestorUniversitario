using ServicioDeDatos;
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

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(PuestoDto))
                return GestorDePuestosDeTrabajo.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPuestos(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro }); 

            if (claseElemento == nameof(RolDto))
                return GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
