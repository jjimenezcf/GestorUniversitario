using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using System.Collections.Generic;
using GestoresDeNegocio.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosDeUnPuestoController :  EntidadController<ContextoSe, PuestosDeUnUsuarioDtm, UsuariosDeUnPuestoDto>
    {

        public UsuariosDeUnPuestoController(GestorDeUsuariosDeUnPuesto gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDeUsuariosDeUnPuesto(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudUsuariosDeUnPuesto(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(UsuarioDto))
                return GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerUsuarios(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            if (claseElemento == nameof(PuestoDto))
                GestorDePuestosDeTrabajo.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPuestos(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
