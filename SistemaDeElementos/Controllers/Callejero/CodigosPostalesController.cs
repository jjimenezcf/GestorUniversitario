using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using MVCSistemaDeElementos.Descriptores.Callejero;
using GestorDeElementos;
using System.Collections.Generic;

namespace SistemaDeElementos.Controllers.Callejero
{
    public class CodigosPostalesController : EntidadController<ContextoSe, CodigoPostalDtm, CodigoPostalDto>
    {

        public CodigosPostalesController(GestorDeCodigosPostales gestorDeCodigosPostales, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeCodigosPostales,
           gestorDeErrores
         )
        {
        }

        public IActionResult CrudCodigosPostales()
        {
            return ViewCrud(new DescriptorDeCodigosPostales(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(PaisDto))
                return GestorDePaises.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPaises(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }

    }
}
