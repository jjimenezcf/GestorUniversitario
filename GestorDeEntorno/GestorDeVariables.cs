using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using System;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public static class FiltrosDeVariables
    {
        public static IQueryable<T> FiltrarPorNombre<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : VariableDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(VariableDto.Nombre))
                    return regristros.Where(x => x.Nombre.Contains(filtro.Valor) || x.Valor.Contains(filtro.Valor));

            return regristros;
        }
    }


    public class GestorDeVariables : GestorDeElementos<CtoEntorno, VariableDtm, VariableDto>
    {

        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<VariableDtm, VariableDto>();
                CreateMap<VariableDto, VariableDtm>();
            }
        }


        public GestorDeVariables(CtoEntorno contexto, IMapper mapeador)
            :base(contexto,mapeador)
        {

        }

        protected override IQueryable<VariableDtm> AplicarFiltros(IQueryable<VariableDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros, parametros);

            return registros
                   .FiltrarPorNombre(filtros);
        }
    }
}
