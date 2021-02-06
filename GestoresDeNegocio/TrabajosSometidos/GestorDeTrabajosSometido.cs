using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeTrabajosSometido : GestorDeElementos<ContextoSe, TrabajoSometidoDtm, TrabajoSometidoDto>
    {

        public class MapearNegocio : Profile
        {
            public MapearNegocio()
            {
                CreateMap<TrabajoSometidoDtm, TrabajoSometidoDto>()
                .ForMember(dto => dto.Ejecutor, dtm => dtm.MapFrom(x => $"({x.Ejecutor.Login})- {x.Ejecutor.Nombre} {x.Ejecutor.Apellido}"))
                .ForMember(dto => dto.InformarA, dtm => dtm.MapFrom(x => x.InformarA.Nombre));

                CreateMap<TrabajoSometidoDto, TrabajoSometidoDtm>()
                .ForMember(dtm => dtm.Ejecutor, dto => dto.Ignore())
                .ForMember(dtm => dtm.InformarA, dto => dto.Ignore());
            }
        }

        public GestorDeTrabajosSometido(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }
        public static GestorDeTrabajosSometido Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTrabajosSometido(contexto, mapeador);
        }



        protected override IQueryable<TrabajoSometidoDtm> AplicarJoins(IQueryable<TrabajoSometidoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.InformarA);
            registros = registros.Include(p => p.Ejecutor);
            return registros;
        }


        protected override void AntesDePersistir(TrabajoSometidoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == TipoOperacion.Insertar)
            {
            }

            if (parametros.Operacion == TipoOperacion.Modificar)
            {
            }

        }

        protected override void AntesDePersistirValidarRegistro(TrabajoSometidoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);
            //Validar que existe dll.clase.metodo o esquema.pa
        }

    }
}

