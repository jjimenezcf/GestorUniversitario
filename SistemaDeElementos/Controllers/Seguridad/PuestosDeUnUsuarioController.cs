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
    public class PuestosDeUnUsuarioController :  RelacionController<ContextoSe, PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>
    {

        public PuestosDeUnUsuarioController(GestorDePuestosDeUnUsuario gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPuestosDeUnUsuario()
        {
            return ViewCrud(new DescriptorDePuestosDeUnUsuario(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(UsuarioDto))
                return GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerUsuarios(posicion, cantidad, filtros);

            if (claseElemento == nameof(PuestoDto))
                return GestorDePuestosDeTrabajo.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPuestos(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }
    }
}
