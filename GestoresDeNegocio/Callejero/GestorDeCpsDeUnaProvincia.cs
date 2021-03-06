﻿using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDeCpsDeUnaProvincia : GestorDeRelaciones<ContextoSe, CpsDeUnaProvinciaDtm, CpsDeUnaProvinciaDto>
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
                    .ForMember(dto => dto.CodigoPostal, dtm => dtm.MapFrom(dtm => dtm.CodigoPostal.Codigo))
                    .ForMember(dto => dto.Provincia, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Codigo}) {dtm.Provincia.Nombre}"));

                CreateMap<CpsDeUnaProvinciaDto, CpsDeUnaProvinciaDtm>()
                    .ForMember(dtm => dtm.CodigoPostal, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Provincia, dto => dto.Ignore());
            }
        }


        public GestorDeCpsDeUnaProvincia(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador, true)
        {
        }


        internal static GestorDeCpsDeUnaProvincia Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCpsDeUnaProvincia(contexto, mapeador);
        }

        public static void CrearRelacion(ContextoSe contexto, CodigoPostalDtm cp, ProvinciaDtm provincia)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            gestor.CrearRelacion(nameof(CpsDeUnaProvinciaDtm.IdCp), cp.Id, provincia.Id, false);
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

            foreach (ClausulaDeFiltrado filtro in filtros.Where(filtro => filtro.Clausula.Equals(nameof(CpsDeUnaProvinciaDtm.CodigoPostal), StringComparison.CurrentCultureIgnoreCase)))
            {
                registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, $"{nameof(CpsDeUnaProvinciaDtm.CodigoPostal)}.{nameof(CodigoPostalDtm.Codigo)}");
            }
            return registros;

        }

        protected override void AntesDePersistirValidarRegistro(CpsDeUnaProvinciaDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);

            var provincia = Contexto.Set<ProvinciaDtm>().LeerCacheadoPorId(registro.IdProvincia);
            var codigoPostal = Contexto.Set<CodigoPostalDtm>().LeerCacheadoPorId(registro.IdCp);

            if (!codigoPostal.Codigo.Substring(0, 2).Equals(provincia.Codigo))
                GestorDeErrores.Emitir($"El código postal {registro.CodigoPostal.Codigo} no se puede relacionar con la provincia {registro.Provincia.Expresion}");
        }

    }
}
