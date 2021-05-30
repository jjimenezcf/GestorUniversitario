using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System;
using GestoresDeNegocio.Archivos;
using Microsoft.EntityFrameworkCore;

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeMunicipios : GestorDeElementos<ContextoSe, MunicipioDtm, MunicipioDto>
    {

        public const string ParametroMunicipio = "csvMunicipio"; 

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<MunicipioDtm, MunicipioDto>()
                    .ForMember(dto => dto.Provincia, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Codigo}) {dtm.Provincia.Nombre}"));

                CreateMap<MunicipioDto, MunicipioDtm>()
                .ForMember(dtm => dtm.Provincia, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());

            }

        }

        public GestorDeMunicipios(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeMunicipios Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeMunicipios(contexto, mapeador); ;
        }

        protected override IQueryable<MunicipioDtm> AplicarJoins(IQueryable<MunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Provincia);
            return registros;
        }
    }
}
