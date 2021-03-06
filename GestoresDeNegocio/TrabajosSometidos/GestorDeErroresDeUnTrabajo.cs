using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using System;
using Gestor.Errores;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeErroresDeUnTrabajo : GestorDeElementos<ContextoSe, ErrorDeUnTrabajoDtm, ErrorDeUnTrabajoDto>
    {

        public class MapeadorErroresDeUnTrabajo : Profile
        {
            public MapeadorErroresDeUnTrabajo()
            {
                CreateMap<ErrorDeUnTrabajoDtm, ErrorDeUnTrabajoDto>()
                .ForMember(dto => dto.TrabajoDeUsuario, dtm => dtm.MapFrom(x => $"({x.TrabajoDeUsuario.Sometedor.Login})- {x.TrabajoDeUsuario.Trabajo.Nombre}"));


                CreateMap<ErrorDeUnTrabajoDto, ErrorDeUnTrabajoDtm>()
                .ForMember(dtm => dtm.TrabajoDeUsuario, dto => dto.Ignore());
            }
        }

        public GestorDeErroresDeUnTrabajo(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeErroresDeUnTrabajo Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeErroresDeUnTrabajo(contexto, mapeador);
        }


        public ErrorDeUnTrabajoDtm CrearError(TrabajoDeUsuarioDtm tu, string error, string detalle)
        {
            var e = new ErrorDeUnTrabajoDtm();
            e.IdTrabajoDeUsuario = tu.Id;
            e.Error = error;
            e.Detalle = detalle;
            e.Fecha = DateTime.Now;
            return PersistirRegistro(e, new ParametrosDeNegocio(TipoOperacion.Insertar));
        }


        protected override IQueryable<ErrorDeUnTrabajoDtm> AplicarJoins(IQueryable<ErrorDeUnTrabajoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.TrabajoDeUsuario);
            registros = registros.Include(p => p.TrabajoDeUsuario.Sometedor);
            registros = registros.Include(p => p.TrabajoDeUsuario.Trabajo);
            return registros;
        }

        internal static void AnotarError(ContextoSe contextoTu, TrabajoDeUsuarioDtm tu, Exception e)
        {
            var gestorEt = Gestor(contextoTu, contextoTu.Mapeador);     
            gestorEt.CrearError(tu, e.Message, GestorDeErrores.Detalle(e));
        }
    }
}

