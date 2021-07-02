using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class ProvinciasController : EntidadController<ContextoSe, ProvinciaDtm, ProvinciaDto>
    {

        public ProvinciasController(GestorDeProvincias gestorDeProvincias, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeProvincias,
           gestorDeErrores
         )
        {
        }

        public IActionResult CrudProvincias()
        {
            return ViewCrud(new DescriptorDeProvincias(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PaisDto))
                return GestorDePaises.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPaises(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}
