using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDeCpsDeUnaProvincia : GestorDeElementos<ContextoSe, CpsDeUnaProvinciaDtm, CpsDeUnaProvinciaDto>
    {
        public class ltrCpsDeUnaProvincia
        {
            internal static readonly string JoinConProvincias = nameof(JoinConProvincias);
            internal static readonly string JoinConCps = nameof(JoinConCps);
        }

        public class MapearPermisosDeUnRol : Profile
        {
            public MapearPermisosDeUnRol()
            {
                CreateMap<CpsDeUnaProvinciaDtm, CpsDeUnaProvinciaDto>()
                    .ForMember(dto => dto.CodigoPostal, dtm => dtm.MapFrom(dtm => dtm.CodigoPostal.cp))
                    .ForMember(dto => dto.Provincia, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Codigo}) {dtm.Provincia.Nombre}"));

                CreateMap<CpsDeUnaProvinciaDto, CpsDeUnaProvinciaDtm>()
                    .ForMember(dtm => dtm.CodigoPostal, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Provincia, dto => dto.Ignore());
            }
        }


        public GestorDeCpsDeUnaProvincia(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }


        internal static GestorDeCpsDeUnaProvincia Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCpsDeUnaProvincia(contexto, mapeador);
        }

        public static void CrearRelacion(ContextoSe contexto, CodigoPostalDtm cp, ProvinciaDtm provincia)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            gestor.CrearRelacion(cp.Id, provincia.Id);
        }


        protected override IQueryable<CpsDeUnaProvinciaDtm> AplicarJoins(IQueryable<CpsDeUnaProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            if (HacerJoinCon(parametros, ltrCpsDeUnaProvincia.JoinConProvincias))
                registros = registros.Include(rp => rp.Provincia);

            if (HacerJoinCon(parametros, ltrCpsDeUnaProvincia.JoinConCps))
                registros = registros.Include(rp => rp.CodigoPostal);
            return registros;
        }


        protected override IQueryable<CpsDeUnaProvinciaDtm> AplicarFiltros(IQueryable<CpsDeUnaProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(CpsDeUnaProvinciaDtm.CodigoPostal).ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(CpsDeUnaProvinciaDtm.CodigoPostal));
            }
            return registros;

        }

    }
}
