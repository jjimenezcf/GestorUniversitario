using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using System;
using Gestor.Elementos.ModeloIu;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;

namespace Gestor.Elementos.Entorno
{

    public static class FiltrosDeVariables
    {
        public static IQueryable<T> FiltrarPorValor<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : VariableDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(VariableDto.Valor).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return regristros.Where(x => x.Valor == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return regristros.Where(x => x.Valor.Contains(filtro.Valor));
                }
            }

            return regristros;
        }
    }


    public class GestorDeVariables : GestorDeElementos<ContextoSe, VariableDtm, VariableDto>
    {

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<VariableDtm, VariableDto>();
                CreateMap<VariableDto, VariableDtm>();
            }
        }

        public GestorDeVariables(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<VariableDtm> AplicarFiltros(IQueryable<VariableDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltrarPorNombre(filtros).FiltrarPorValor(filtros);
        }

        protected override void AntesMapearRegistroParaModificar(VariableDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaModificar(elemento, opciones);
            new CacheDeVariable(Contexto).BorrarCache(elemento.Nombre);
        }

        protected override void AntesMapearRegistroParaEliminar(VariableDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaEliminar(elemento, opciones);
            new CacheDeVariable(Contexto).BorrarCache(elemento.Nombre);
        }
    }
}
