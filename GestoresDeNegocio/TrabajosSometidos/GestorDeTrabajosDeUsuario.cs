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
using Enumerados;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class EnumParametroTu : EnumParametro
    {
        public static string terminando = nameof(terminando);
    }

    public class EntornoDeTrabajo
    {
        public GestorDeTrabajosDeUsuario GestorDeEntorno { get; private set; }
        public TrabajoDeUsuarioDtm Trabajo { get; private set; }
        public ContextoSe contextoPr { get; set; }

        public bool HayErrores
        {
            get
            {
                var gestor = GestorDeErroresDeUnTrabajo.Gestor(GestorDeEntorno.Contexto, GestorDeEntorno.Contexto.Mapeador);
                var filtro = new ClausulaDeFiltrado { Clausula = nameof(ErrorDeUnTrabajoDtm.IdTrabajoDeUsuario), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = Trabajo.Id.ToString() };
                return gestor.LeerRegistros(1, 1, new List<ClausulaDeFiltrado> { filtro }).Count > 0;
            }
        }

        public EntornoDeTrabajo(GestorDeTrabajosDeUsuario gestor, TrabajoDeUsuarioDtm trabajoUsuario)
        {
            GestorDeEntorno = gestor;
            Trabajo = trabajoUsuario;
        }

        public int AnotarTraza(string traza)
        {
            return AnotarTraza(0, traza);
        }

        public int AnotarTraza(int id, string traza)
        {
            return GestorDeTrazasDeUnTrabajo.AnotarTraza(GestorDeEntorno.Contexto, Trabajo, id, traza);
        }

        internal void ModificarUltimaTraza(string traza)
        {
            TrazaDeUnTrabajoDtm registroDeTraza =  GestorDeTrazasDeUnTrabajo.LeerUltimaTraza(GestorDeEntorno.Contexto, Trabajo.Id);
            AnotarTraza(registroDeTraza.Id, traza);
        }

        public void AnotarError(Exception e)
        {
            GestorDeErroresDeUnTrabajo.AnotarError(GestorDeEntorno.Contexto, Trabajo, e);
        }

        public bool IniciarTransaccion()
        {
            return GestorDeEntorno.IniciarTransaccion();
        }
        public void RollBack(bool transaccion)
        {
            GestorDeEntorno.Rollback(transaccion);
        }
        public void Commit(bool transaccion)
        {
            GestorDeEntorno.Commit(transaccion);
        }

        public void PonerSemaforo()
        {
            GestorDeSemaforoDeTrabajos.PonerSemaforo(Trabajo);
            AnotarTraza($"Trabajo iniciado por el usuario {GestorDeEntorno.Contexto.DatosDeConexion.Login}");
        }

        public void QuitarSemaforo(string traza)
        {
            GestorDeSemaforoDeTrabajos.QuitarSemaforo(Trabajo);
            AnotarTraza(traza);
        }

    }


    public class GestorDeTrabajosDeUsuario : GestorDeElementos<ContextoSe, TrabajoDeUsuarioDtm, TrabajoDeUsuarioDto>
    {

        public static string SolicitudDeUsuario = nameof(SolicitudDeUsuario);

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

        protected override void AntesMapearRegistroParaInsertar(TrabajoDeUsuarioDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaInsertar(elemento, opciones);
            if (elemento.Estado.IsNullOrEmpty())
                elemento.Estado = enumEstadosDeUnTrabajo.Pendiente.ToDto();
            if (elemento.Parametros.IsNullOrEmpty())
                elemento.Parametros = "[]";
        }

        internal static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoSometidoDtm ts, string parametros)
        {
            var tu = new TrabajoDeUsuarioDtm();
            tu.IdSometedor = contexto.DatosDeConexion.IdUsuario;
            tu.IdEjecutor = ts.IdEjecutor == null ? tu.IdSometedor : (int)ts.IdEjecutor;
            tu.IdTrabajo = ts.Id;
            tu.Estado = enumEstadosDeUnTrabajo.Pendiente.ToDtm();
            tu.Planificado = DateTime.Now;
            tu.Parametros = parametros;
            return Crear(contexto, tu);
        }

        private static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoDeUsuarioDtm tu)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            tu = gestor.PersistirRegistro(tu, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            return tu;
        }

        public static void Iniciar(ContextoSe contextoTu, int idTrabajoDeUsuario)
        {
            var gestorTu = Gestor(contextoTu, contextoTu.Mapeador);
            var tu = gestorTu.LeerRegistroPorId(idTrabajoDeUsuario, false);
            var entorno = new EntornoDeTrabajo(gestorTu, tu);

            entorno.PonerSemaforo();
            var tran = entorno.IniciarTransaccion();

            try
            {
                tu.Iniciado = DateTime.Now;
                tu.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.iniciado);
                tu = entorno.GestorDeEntorno.PersistirRegistro(tu, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                entorno.Commit(tran);
            }
            catch (Exception e)
            {
                entorno.RollBack(tran);
                entorno.AnotarError(e);
                entorno.QuitarSemaforo("Iniciación cancelada");
                throw;
            }

            EjecutarTrabajo(entorno);
        }

        private static bool EjecutarTrabajo(EntornoDeTrabajo entorno)
        {
            bool tran = entorno.GestorDeEntorno.IniciarTransaccion();
            try
            {
                var metodo = GestorDeTrabajosSometido.ValidarExisteTrabajoSometido(entorno.Trabajo.Trabajo);
                using (var contextoPr = ContextoSe.ObtenerContexto(entorno.GestorDeEntorno.Contexto))
                {
                    entorno.contextoPr = contextoPr;
                    metodo.Invoke(null, new object[] { entorno });
                }
                entorno.Trabajo.Estado = !entorno.HayErrores
                    ? TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Terminado)
                    : TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.conErrores);
            }
            catch (Exception e)
            {
                entorno.Trabajo.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Error);
                if (e.InnerException != null)
                {
                    entorno.AnotarError(e.InnerException);
                    throw e.InnerException;
                }

                entorno.AnotarError(e);

                throw;
            }
            finally
            {
                entorno.Trabajo.Terminado = DateTime.Now;
                var parametros = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                parametros.Parametros[EnumParametro.accion] = EnumParametroTu.terminando;
                entorno.GestorDeEntorno.PersistirRegistro(entorno.Trabajo, parametros);
                entorno.GestorDeEntorno.Commit(tran);
                entorno.QuitarSemaforo($"Trabajo finalizado: {(entorno.Trabajo.Estado == TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Terminado) ? "sin errores" : "con errores")}");
            }

            return tran;
        }

        public static void Bloquear(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario, false);
            try
            {
                if (tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Pendiente))
                    throw new Exception($"El trabajo no se puede bloquear, ha de estar en estado pendiente y está en estado {TrabajoSometido.ToDto(tu.Estado)}");
                tu.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Bloqueado);
                gestor.PersistirRegistro(tu, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tu, $"Trabajo bloqueado por el usuario {contexto.DatosDeConexion.Login}");
            }
            catch (Exception e)
            {
                GestorDeErroresDeUnTrabajo.AnotarError(contexto, tu, e);
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tu, $"El usuario {contexto.DatosDeConexion.Login} no ha podido bloquear el trabajo");
                throw;
            }
        }

        public static void Desbloquear(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario, false);
            try
            {
                if (tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Bloqueado))
                    throw new Exception($"El trabajo no se puede desbloquear, ha de estar en estado bloqueado y está en estado {TrabajoSometido.ToDto(tu.Estado)}");
                tu.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Pendiente);
                gestor.PersistirRegistro(tu, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tu, $"Trabajo desbloqueado por el usuario {contexto.DatosDeConexion.Login}");
            }
            catch (Exception e)
            {
                GestorDeErroresDeUnTrabajo.AnotarError(contexto, tu, e);
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tu, $"El usuario {contexto.DatosDeConexion.Login} no ha podido desbloquear el trabajo");
                throw;
            }
        }

        public static void Resometer(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario, false);

            if (tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Error) &&
                tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.conErrores) &&
                tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Terminado) &&
                tu.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.iniciado)
               )
                throw new Exception($"El trabajo no se puede resometer, ha de estar en estado terminado, iniciado, con errores o erroneo y está en estado {TrabajoSometido.ToDto(tu.Estado)}");

            var tr = new TrabajoDeUsuarioDtm();
            tr.IdSometedor = contexto.DatosDeConexion.IdUsuario;
            tr.IdEjecutor = tu.IdEjecutor;
            tr.IdTrabajo = tu.IdTrabajo;
            tr.Estado = enumEstadosDeUnTrabajo.Pendiente.ToDtm();
            tr.Planificado = DateTime.Now;
            tr.Parametros = tu.Parametros;
            Crear(contexto, tr);
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

            if (parametros.Operacion == enumTipoOperacion.Insertar && parametros.Parametros.ContainsKey(SolicitudDeUsuario) && (bool)parametros.Parametros[SolicitudDeUsuario] && !Contexto.DatosDeConexion.EsAdministrador)
                GestorDeErrores.Emitir("Un usuario no administrador no puede solicitar crear un trabajo sometido directamente desde las interface");

            if (parametros.Operacion == enumTipoOperacion.Modificar)
                ValidarAntesDeModificar(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
                ValidarAntesDeEliminar(registro, parametros);
        }

        
        private void ValidarAntesDeEliminar(TrabajoDeUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            var RegistroEnBD = ((TrabajoDeUsuarioDtm)parametros.registroEnBd);
            if (RegistroEnBD.Iniciado.HasValue && !RegistroEnBD.Terminado.HasValue)
            {
                GestorDeErrores.Emitir("Un trabajo en ejecución no se puede eliminar");
            }
        }

        private void ValidarAntesDeModificar(TrabajoDeUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            var RegistroEnBD = ((TrabajoDeUsuarioDtm)parametros.registroEnBd);
            if (RegistroEnBD.IdSometedor != registro.IdSometedor)
                GestorDeErrores.Emitir("No se puede modificar el sometedor de un trabajo");

            if (RegistroEnBD.Encolado != registro.Encolado)
                GestorDeErrores.Emitir("No se puede modificar la fecha de entrada de un trabajo en la cola");

            if (!registro.Iniciado.HasValue && registro.Terminado.HasValue)
                GestorDeErrores.Emitir("No se se puede terminar un trabajo que aun no se ha iniciado");

            if (registro.Terminado.HasValue && !SeEstaTerminando(parametros.Parametros))
                GestorDeErrores.Emitir("No se se puede modificar un trabajo terminado");

            if (RegistroEnBD.Iniciado.HasValue && !SeEstaTerminando(parametros.Parametros))
                GestorDeErrores.Emitir("Un trabajo en ejecución no se puede modificar");
        }

        private bool SeEstaTerminando(Dictionary<string, object> parametros)
        {
            if (!parametros.ContainsKey(EnumParametro.accion))
                return false;

            return (string)parametros[EnumParametro.accion] == EnumParametroTu.terminando;
        }

        protected override void AntesDePersistir(TrabajoDeUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                registro.Encolado = DateTime.Now;
            }

            if (parametros.Operacion == enumTipoOperacion.Insertar || parametros.Operacion == enumTipoOperacion.Modificar)
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

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                GestorDeTrazasDeUnTrabajo.EliminarTrazas(Contexto, ((TrabajoDeUsuarioDtm)parametros.registroEnBd).Id);
                GestorDeErroresDeUnTrabajo.EliminarErrores(Contexto, ((TrabajoDeUsuarioDtm)parametros.registroEnBd).Id);
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
/*
 * 
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
 * */
