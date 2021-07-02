using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using GestoresDeNegocio.Negocio;
using ModeloDeDto.Negocio;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using System.Collections.Generic;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class TrabajosSometidoController : EntidadController<ContextoSe, TrabajoSometidoDtm, TrabajoSometidoDto>
    {

        public TrabajosSometidoController(GestorDeTrabajosSometido gestorDeTrabajos, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeTrabajos, 
          gestorDeErrores
        )
        {

        }

        
        public IActionResult CrudDeTrabajosSometido()
        {
            return ViewCrud(new DescriptorDeTrabajosSometido(Contexto, ModoDescriptor.Mantenimiento));
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
