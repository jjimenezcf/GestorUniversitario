using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using GestorDeElementos;

namespace GestoresDeNegocio.Entorno
{

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
        internal static GestorDeVariables Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeVariables(contexto, mapeador);
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

        protected override IQueryable<VariableDtm> AplicarFiltros(IQueryable<VariableDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(VariableDto.Valor).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.Valor == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.Valor.Contains(filtro.Valor));
                }
            }

            return registros;
        }

    }
}
