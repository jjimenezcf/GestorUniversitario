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
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using Gestor.Errores;
using ServicioDeDatos.Elemento;

namespace GestoresDeNegocio.TrabajosSometidos
{
    public class Parametro
    {
        public string parametro { get; set; }
        public object valor { get; set; }
    }
    public class ParametrosJson
    {
        public List<Parametro> Parametros { get; private set; }
        public ParametrosJson(string json)
        {
            try
            {
                ValidarJson(json);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("The free-quota limit of 10 schema generations per hour has been reached"))
                    throw;
            }
            Parametros = JsonConvert.DeserializeObject<List<Parametro>>(json);
        }

        public static void ValidarJson(string json)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(List<Parametro>));
            try
            {
                JArray actualJson = JArray.Parse(json);
                bool valid = actualJson.IsValid(schema, out IList<string> errorMessages);

                if (!valid)
                {
                    var mensaje = "";
                    foreach (var me in errorMessages)
                    {
                        mensaje = $"{mensaje}{Environment.NewLine}{me}";
                    }
                    GestorDeErrores.Emitir($"Parámetros Json mal definido.{Environment.NewLine}{json}{Environment.NewLine}{mensaje}");
                }
            }
            catch (Exception exc)
            {
                GestorDeErrores.Emitir($"Json mal definido.{Environment.NewLine}{json}", exc);
            }
        }
    }


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
                .ForMember(dtm => dtm.Trabajo, dto => dto.Ignore())
                .ForMember(dtm => dtm.Estado, dto => dto.MapFrom(x => TrabajoSometido.ToDtm(x.Estado)));
            }
        }

        public GestorDeTrabajosDeUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeTrabajosDeUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTrabajosDeUsuario(contexto, mapeador); ;
        }

        internal static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoSometidoDtm ts, string parametros)
        {
            var tu = new TrabajoDeUsuarioDtm();
            tu.IdSometedor = contexto.DatosDeConexion.IdUsuario;
            tu.IdEjecutor = ts.IdEjecutor == null ? tu.IdSometedor : (int)ts.IdEjecutor;
            tu.IdTrabajo = ts.Id;
            tu.Estado = enumEstadosDeUnTrabajo.pendiente.ToDtm();
            tu.Encolado = DateTime.Now;
            tu.Planificado = tu.Encolado;
            tu.Parametros = parametros;
            var gestor = Gestor(contexto, contexto.Mapeador);
            tu = gestor.PersistirRegistro(tu, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return tu;
        }

        public static void Iniciar(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var transaccion = contexto.IniciarTransaccion();
            try
            {
                var i = contexto.Database.ExecuteSqlInterpolated($@"UPDATE TRABAJO.USUARIO 
                                                        SET 
                                                          INICIADO = GETDATE(), 
                                                          ESTADO = {TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.iniciado)}
                                                        WHERE 
                                                          ID = {idTrabajoDeUsuario}
                                                          AND INICIADO IS NULL 
                                                          AND ESTADO LIKE {TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.pendiente)}
                                                       ");

                if (i > 0)
                    contexto.Commit(transaccion);
                else
                    throw new Exception("El trabajo ya estaba iniciado");
            }
            catch
            {
                contexto.Rollback(transaccion);
                throw;
            }
        }

        public static void Bloquear(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var transaccion = contexto.IniciarTransaccion();
            try
            {
                var i = contexto.Database.ExecuteSqlInterpolated($@"UPDATE TRABAJO.USUARIO 
                                                        SET  
                                                          ESTADO = {TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Bloqueado)}
                                                        WHERE 
                                                          ID = {idTrabajoDeUsuario}
                                                          AND INICIADO IS NULL 
                                                          AND ESTADO LIKE {TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.pendiente)}
                                                       ");
                if (i > 0)
                    contexto.Commit(transaccion);
                else
                    throw new Exception($"El trabajo no se puede bloquear");
            }
            catch
            {
                contexto.Rollback(transaccion);
                throw;
            }
        }
        public static void Terminar(ContextoSe contexto, TrabajoDeUsuarioDtm tu)
        {
            var transaccion = contexto.IniciarTransaccion();
            try
            {
                var i = contexto.Database.ExecuteSqlInterpolated($@"UPDATE TRABAJO.USUARIO 
                                                        SET 
                                                          TERMINADO = GETDATE(), 
                                                          ESTADO = '{TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.terminado)}'
                                                        WHERE 
                                                          ID = {tu.Id}
                                                          AND INICIADO IS NOT NULL 
                                                          AND TERMINADO IS NULL 
                                                          AND ESTADO LIKE '{TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.iniciado)}'
                                                       ");
                if (i > 0)
                    contexto.Commit(transaccion);
                else
                    throw new Exception($"El trabajo ya estaba terminado");
            }
            catch
            {
                contexto.Rollback(transaccion);
                throw;
            }
        }

        protected override IQueryable<TrabajoDeUsuarioDtm> AplicarJoins(IQueryable<TrabajoDeUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Ejecutor);
            registros = registros.Include(p => p.Sometedor);
            registros = registros.Include(p => p.Trabajo);
            return registros;
        }

        protected override void AntesDePersistirValidarRegistro(TrabajoDeUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);

            if (parametros.Operacion == TipoOperacion.Eliminar || parametros.Operacion == TipoOperacion.Modificar)
            {
                if (RegistroEnBD.Iniciado.HasValue)
                    GestorDeErrores.Emitir("Un trabajo en ejecución o finalizado no se puede suprimir ni modificar");
            }

            if (parametros.Operacion == TipoOperacion.Modificar)
            {
                if (RegistroEnBD.IdSometedor != registro.IdSometedor)
                    GestorDeErrores.Emitir("No se puede modificar el sometedor de un trabajo");
                if (RegistroEnBD.Encolado != registro.Encolado)
                    GestorDeErrores.Emitir("No se puede modificar la fecha de entrada de un trabajo en la cola");
                if (!registro.Iniciado.HasValue && registro.Terminado.HasValue)
                    GestorDeErrores.Emitir("No se se puede terminar un trabajo que aun no se ha iniciado");
            }

        }

        protected override void AntesDePersistir(TrabajoDeUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == TipoOperacion.Insertar || parametros.Operacion == TipoOperacion.Modificar)
            {
                if (!registro.Iniciado.HasValue)
                {
                    new ParametrosJson(registro.Parametros);
                    if (registro.Planificado.Millisecond > 0 || registro.Planificado.Second > 0)
                    {
                        registro.Planificado = registro.Planificado.AddMilliseconds(1000 - registro.Planificado.Millisecond);
                        registro.Planificado = registro.Planificado.AddSeconds(60 - registro.Planificado.Second);
                        registro.Planificado.AddMinutes(1);
                    }
                }
            }
        }

    }
}

//Antigua forma, antes de usar Dapper
//using (var c = ContextoSe.ObtenerContexto())
//{
//    if (!new ExistePa(c, registro.Pa, registro.Esquema).Existe)
//        GestorDeErrores.Emitir($"El {registro.Esquema}.{registro.Pa} indicado no existe en la BD");
//}

