using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using System;
using Utilidades;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeTrabajosDeUsuario : GestorDeElementos<ContextoSe, TrabajoDeUsuarioDtm, TrabajoDeUsuarioDto>
    {

        public class MapearNegocio : Profile
        {
            public MapearNegocio()
            {
                CreateMap<TrabajoDeUsuarioDtm, TrabajoDeUsuarioDto>()
                .ForMember(dto => dto.Ejecutor, dtm => dtm.MapFrom(x => $"({x.Ejecutor.Login})- {x.Ejecutor.Nombre} {x.Ejecutor.Apellido}"))
                .ForMember(dto => dto.Trabajo, dtm => dtm.MapFrom(x => x.Trabajo.Nombre))
                .ForMember(dto => dto.Ejecutor, dtm => dtm.MapFrom(x => $"({x.Ejecutor.Login}) {x.Ejecutor.Apellido} {x.Ejecutor.Nombre}"))
                .ForMember(dto => dto.Sometedor, dtm => dtm.MapFrom(x => $"({x.Sometedor.Login}) {x.Sometedor.Apellido} {x.Sometedor.Nombre}"))
                .ForMember(dto => dto.Estado, dtm => dtm.MapFrom(x => TrabajoSometido.ToDto(x.Estado)));


                CreateMap<TrabajoDeUsuarioDto, TrabajoDeUsuarioDtm>()
                .ForMember(dtm => dtm.Ejecutor, dto => dto.Ignore())
                .ForMember(dtm => dtm.Sometedor, dto => dto.Ignore())
                .ForMember(dtm => dtm.Sometedor, dto => dto.Ignore());
            }
        }

        public GestorDeTrabajosDeUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }
        public GestorDeTrabajosDeUsuario(ContextoSe contexto)
        : base(contexto)
        {

        }

        public static GestorDeTrabajosDeUsuario Gestor(ContextoSe contexto)
        {
            return new GestorDeTrabajosDeUsuario(contexto);
        }

        internal static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoSometidoDtm ts, string parametros)
        {
            var gestor = Gestor(contexto);
            var tu = new TrabajoDeUsuarioDtm();
            tu.IdSometedor = contexto.DatosDeConexion.IdUsuario;
            tu.IdEjecutor = ts.IdEjecutor == null ? tu.IdSometedor  : (int)ts.IdEjecutor;
            tu.IdTrabajo = ts.Id;
            tu.Estado = enumEstadosDeUnTrabajo.pendiente.ToDtm();
            tu.Encolado = DateTime.Now;
            tu.Planificado = DateTime.Now;
            tu.Parametros = parametros;
            tu = gestor.PersistirRegistro(tu, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return tu;
        }

        protected override IQueryable<TrabajoDeUsuarioDtm> AplicarJoins(IQueryable<TrabajoDeUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Ejecutor);
            registros = registros.Include(p => p.Sometedor);
            registros = registros.Include(p => p.Trabajo);
            return registros;
        }

    }
}

//Antigua forma, antes de usar Dapper
//using (var c = ContextoSe.ObtenerContexto())
//{
//    if (!new ExistePa(c, registro.Pa, registro.Esquema).Existe)
//        GestorDeErrores.Emitir($"El {registro.Esquema}.{registro.Pa} indicado no existe en la BD");
//}

