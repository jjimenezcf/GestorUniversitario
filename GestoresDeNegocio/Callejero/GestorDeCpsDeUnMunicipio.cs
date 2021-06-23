using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDeCpsDeUnMunicipio : GestorDeRelaciones<ContextoSe, CpsDeUnMunicipioDtm, CpsDeUnMunicipioDto>
    {
        public class ltrCpsDeUnMunicipio
        {
            internal static readonly string JoinConMunicipios = nameof(JoinConMunicipios);
            internal static readonly string JoinConCps = nameof(JoinConCps);
        }

        public class MapearPermisosDeUnRol : Profile
        {
            public MapearPermisosDeUnRol()
            {
                CreateMap<CpsDeUnMunicipioDtm, CpsDeUnMunicipioDto>()
                    .ForMember(dto => dto.CodigoPostal, dtm => dtm.MapFrom(dtm => dtm.CodigoPostal.Codigo))
                    .ForMember(dto => dto.Municipio, dtm => dtm.MapFrom(dtm => $"({dtm.Municipio.Codigo}) {dtm.Municipio.Nombre}"));

                CreateMap<CpsDeUnMunicipioDto, CpsDeUnMunicipioDtm>()
                    .ForMember(dtm => dtm.CodigoPostal, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Municipio, dto => dto.Ignore());
            }
        }


        public GestorDeCpsDeUnMunicipio(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador, true)
        {
        }


        internal static GestorDeCpsDeUnMunicipio Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCpsDeUnMunicipio(contexto, mapeador);
        }

        public static void CrearRelacion(ContextoSe contexto, CodigoPostalDtm cp, MunicipioDtm municipioDtm)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            gestor.CrearRelacion(cp.Id, municipioDtm.Id, false);
        }

        internal static void CrearRelacionConMunicipioSiNoExiste(ContextoSe contexto, CodigoPostalDtm codigoPostalDtm,string iso2Pais, string provincia,  string municipio)
        {
            var municipioDtm = GestorDeMunicipios.LeerMunicipioPorNombre(contexto, iso2Pais, provincia, municipio, paraActualizar: false, errorSiNoHay: false, errorSiMasDeUno: true);
            if (municipioDtm != null)
                CrearRelacion(contexto, codigoPostalDtm, municipioDtm);
        }


        protected override IQueryable<CpsDeUnMunicipioDtm> AplicarJoins(IQueryable<CpsDeUnMunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            if (HacerJoinCon(parametros, ltrCpsDeUnMunicipio.JoinConMunicipios))
                registros = registros.Include(rp => rp.Municipio);

            if (HacerJoinCon(parametros, ltrCpsDeUnMunicipio.JoinConCps))
                registros = registros.Include(rp => rp.CodigoPostal);
            return registros;
        }


        protected override IQueryable<CpsDeUnMunicipioDtm> AplicarFiltros(IQueryable<CpsDeUnMunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(CpsDeUnMunicipioDtm.CodigoPostal).ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(CpsDeUnMunicipioDtm.CodigoPostal));
            }
            return registros;

        }
    }
}
