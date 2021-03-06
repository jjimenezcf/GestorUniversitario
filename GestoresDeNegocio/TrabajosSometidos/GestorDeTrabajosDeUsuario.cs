﻿using System.Collections.Generic;
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
using ServicioDeDatos.Entorno;
using ModeloDeDto;
using System.Threading.Tasks;
using Dapper;
using GestoresDeNegocio.Entorno;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class EnumParametroTu : EnumParametro
    {
        public static string terminando = nameof(terminando);
    }

    public class ResultadoDelProceso
    {
        public bool Resultado { get; }
        public ResultadoDelProceso(bool resultado)
        {
            Resultado = resultado;
        }
    }

    public class EntornoDeTrabajo
    {
        public GestorDeTrabajosDeUsuario GestorDelEntorno { get; }
        public TrabajoDeUsuarioDtm TrabajoDeUsuario { get; }
        public ContextoSe contextoDelProceso { get; set; }
        public ContextoSe ContextoDelEntorno => GestorDelEntorno.Contexto;

        public bool HayErrores
        {
            get
            {
                var gestor = GestorDeErroresDeUnTrabajo.Gestor(ContextoDelEntorno, ContextoDelEntorno.Mapeador);
                var filtro = new ClausulaDeFiltrado { Clausula = nameof(ErrorDeUnTrabajoDtm.IdTrabajoDeUsuario), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = TrabajoDeUsuario.Id.ToString() };
                return gestor.LeerRegistros(1, 1, new List<ClausulaDeFiltrado> { filtro }).Count > 0;
            }
        }

        public bool ProcesoIniciadoPorLaCola { get; set; }
        public UsuarioDtm Sometedor { get; }
        public UsuarioDtm Ejecutor { get; }
        public TrabajoSometidoDtm TrabajoSometido { get; }

        public EntornoDeTrabajo(GestorDeTrabajosDeUsuario gestorDelEntorno, TrabajoDeUsuarioDtm trabajoUsuario)
        {
            TrabajoDeUsuario = trabajoUsuario;
            gestorDelEntorno.Contexto.EsElContextosDeUnEntorno = true;
            TrabajoSometido = gestorDelEntorno.Contexto.Set<TrabajoSometidoDtm>().AsNoTracking().First(p => p.Id.Equals(trabajoUsuario.IdTrabajo));
            Sometedor = gestorDelEntorno.Contexto.Set<UsuarioDtm>().AsNoTracking().First(p => p.Id.Equals(trabajoUsuario.IdSometedor));
            Ejecutor = gestorDelEntorno.Contexto.Set<UsuarioDtm>().AsNoTracking().First(p => p.Id.Equals(trabajoUsuario.IdEjecutor));
            gestorDelEntorno.Contexto.NombreDelTrabajo = TrabajoSometido.Nombre;
            GestorDelEntorno = gestorDelEntorno;
        }

        public TrazaDeUnTrabajoDtm CrearTraza(string traza)
        {
            return GestorDeTrazasDeUnTrabajo.AnotarTraza(ContextoDelEntorno, TrabajoDeUsuario, traza);
        }

        public TrazaDeUnTrabajoDtm ActualizarTraza(TrazaDeUnTrabajoDtm trazaDtm, string traza)
        {
            trazaDtm.Traza = traza;
            return GestorDeTrazasDeUnTrabajo.ActualizarTraza(ContextoDelEntorno, trazaDtm);
        }

        public void AnotarError(Exception e)
        {
            GestorDeErroresDeUnTrabajo.AnotarError(ContextoDelEntorno, TrabajoDeUsuario, e);
        }

        public void AnotarError(string error, Exception e)
        {
            GestorDeErroresDeUnTrabajo.CrearError(ContextoDelEntorno, TrabajoDeUsuario, error, GestorDeErrores.Detalle(e));
        }

        public bool IniciarTransaccion()
        {
            return GestorDelEntorno.IniciarTransaccion();
        }
        public void RollBack(bool transaccion)
        {
            GestorDelEntorno.Rollback(transaccion);
        }
        public void Commit(bool transaccion)
        {
            GestorDelEntorno.Commit(transaccion);
        }

        public void PonerSemaforo()
        {
            GestorDeSemaforoDeTrabajos.PonerSemaforo(TrabajoDeUsuario, ContextoDelEntorno.DatosDeConexion.Login);
            CrearTraza($"Trabajo iniciado por el usuario {ContextoDelEntorno.DatosDeConexion.Login}");
        }

        public void QuitarSemaforo(string traza)
        {
            GestorDeSemaforoDeTrabajos.QuitarSemaforo(TrabajoDeUsuario);
            CrearTraza(traza);
        }

        internal void ComunicarError(Exception e)
        {
            AnotarError(e);
            if (TrabajoSometido.ComunicarError)
            {
                GestorDeCorreos.CrearCorreoPara(ContextoDelEntorno
                    , new List<string> { Sometedor.eMail }
                    , $"Error al ejecutar el trabajo {TrabajoSometido.Nombre}"
                    , $"Error en la ejecución del trabajo {TrabajoSometido.Nombre} de fecha {TrabajoDeUsuario.Encolado}, acceda al mantenimiento de trabajos de usuario para visualizar los errores"
                    , new List<TipoDtoElmento> { new TipoDtoElmento { TipoDto = typeof(TrabajoDeUsuarioDto).FullName, IdElemento = TrabajoDeUsuario.Id, Referencia = TrabajoSometido.Nombre } }
                    , null);
            }
        }

        internal void ComunicarFinalizacion()
        {
            if (TrabajoSometido.ComunicarFin)
            {
                GestorDeCorreos.CrearCorreoPara(ContextoDelEntorno
                    , new List<string> { Sometedor.eMail }
                    , $"Trabajo {TrabajoSometido.Nombre} finalizado{(TrabajoDeUsuario.Estado == enumEstadosDeUnTrabajo.conErrores.ToDtm() ? " con errores" : "")}"
                    , $"El trabajo {TrabajoSometido.Nombre} de fecha {TrabajoDeUsuario.Encolado} ha finalizado, acceda a la traza{(TrabajoDeUsuario.Estado == enumEstadosDeUnTrabajo.conErrores.ToDtm() ? " y a los errores" : "")} para ver el resultado"
                    , new List<TipoDtoElmento> { new TipoDtoElmento { TipoDto = typeof(TrabajoDeUsuarioDto).FullName, IdElemento = TrabajoDeUsuario.Id, Referencia = TrabajoSometido.Nombre } }
                    , null);
            }
        }
    }

    public static class GestorDeTrabajosDeUsuarioExtension
    {
        public static Task ProcesarCola(this GestorDeTrabajosDeUsuario gestorDelEntorno, UsuarioDtm usuario)
        {
            CumplimentarDatosDeConexion(gestorDelEntorno, usuario);

            var trabajosPorEjecutar = LeerTrabajoPendiente();
            if (trabajosPorEjecutar.Count == 1)
            {
                bool trazar = CacheDeVariable.Cola_Trazar;
                if (trazar) gestorDelEntorno.Contexto.IniciarTraza(trabajosPorEjecutar[0].Nombre);
                try
                {
                    GestorDeTrabajosDeUsuario.Iniciar(gestorDelEntorno.Contexto, trabajosPorEjecutar[0].Id, true);
                    if (trazar) gestorDelEntorno.Contexto.CerrarTraza("Trabajo finalizado correctamente");
                }
                catch
                {
                    if (trazar) gestorDelEntorno.Contexto.CerrarTraza("Trabajo finalizado con errores");
                }
            }


            var gestorDeCorreos = GestorDeCorreos.Gestor(gestorDelEntorno.Contexto, gestorDelEntorno.Contexto.Mapeador);
            gestorDeCorreos.EnviarCorreoPendientesAsync();

            TrabajosDeEntorno.SometerBorrarTrazas(gestorDelEntorno.Contexto);

            return Task.FromResult(new ResultadoDelProceso(true));
        }

        private static List<TrabajoDeUsuarioDapper> LeerTrabajoPendiente()
        {
            var consulta = new ConsultaSql<TrabajoDeUsuarioDapper>(TrabajosDeUsuarioSql.LeerTrabajoPendiente);
            var trabajos = consulta.LanzarConsulta(new DynamicParameters(null));
            return trabajos;
        }


        private static void CumplimentarDatosDeConexion(GestorDeTrabajosDeUsuario gestor, UsuarioDtm usuario)
        {
            gestor.Contexto.DatosDeConexion.IdUsuario = usuario.Id;
            gestor.Contexto.DatosDeConexion.EsAdministrador = usuario.EsAdministrador;
            gestor.Contexto.DatosDeConexion.Login = usuario.Login;

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
                .ForMember(dto => dto.Trabajo, dtm => dtm.MapFrom(x => x.Trabajo.Nombre))
                .ForMember(dto => dto.Ejecutor, dtm => dtm.MapFrom(x => $"({x.Ejecutor.Login})- {x.Ejecutor.Nombre} {x.Ejecutor.Apellido}"))
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

        public static GestorDeTrabajosDeUsuario Gestor(ContextoSe contexto)
        {
            return new GestorDeTrabajosDeUsuario(contexto, contexto.Mapeador);
        }

        internal static TrabajoDeUsuarioDtm CrearSiNoEstaPendiente(ContextoSe contexto, TrabajoSometidoDtm ts, Dictionary<string, object> datosDeCreacion)
        {
            var filtro1 = new ClausulaDeFiltrado { Clausula = nameof(TrabajoDeUsuarioDtm.IdTrabajo), Criterio = CriteriosDeFiltrado.igual, Valor = ts.Id.ToString() };
            var filtro2 = new ClausulaDeFiltrado { Clausula = nameof(TrabajoDeUsuarioDtm.Estado), Criterio = CriteriosDeFiltrado.igual, Valor = enumEstadosDeUnTrabajo.Pendiente.ToDtm() };
            var filtros = new List<ClausulaDeFiltrado> { filtro1, filtro2 };
            var parametros = new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo, aplicarJoin: false);
            var gestor = Gestor(contexto);
            var trabajosDtm = gestor.LeerRegistros(0, -1, filtros, null, null, parametros);
            if (trabajosDtm.Count == 0)
                return Crear(contexto, ts, datosDeCreacion);

            return trabajosDtm[0];
        }

        internal static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoSometidoDtm ts, Dictionary<string, object> datosDeCreacion)
        {
            var tu = new TrabajoDeUsuarioDtm();
            tu.IdSometedor = contexto.DatosDeConexion.IdUsuario;
            tu.IdEjecutor = ts.IdEjecutor == null ? tu.IdSometedor : (int)ts.IdEjecutor;
            tu.IdTrabajo = ts.Id;
            tu.Estado = enumEstadosDeUnTrabajo.Pendiente.ToDtm();
            tu.Planificado = datosDeCreacion.ContainsKey(nameof(TrabajoDeUsuarioDtm.Planificado)) ? (DateTime)datosDeCreacion[nameof(TrabajoDeUsuarioDtm.Planificado)] : DateTime.Now;
            tu.Parametros = datosDeCreacion.ContainsKey(nameof(TrabajoDeUsuarioDtm.Parametros)) ? datosDeCreacion[nameof(TrabajoDeUsuarioDtm.Parametros)].ToString() : new List<string>().ToJson();
            return Crear(contexto, tu);
        }

        private static TrabajoDeUsuarioDtm Crear(ContextoSe contexto, TrabajoDeUsuarioDtm tu)
        {
            var gestor = Gestor(contexto);
            tu = gestor.PersistirRegistro(tu, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            return tu;
        }

        public static void Iniciar(ContextoSe contextoDelEntorno, int idTrabajoDeUsuario, bool iniciadoPorLaCola)
        {
            var gestorDelEntorno = Gestor(contextoDelEntorno);
            var trabajoDeUsuarioDtm = gestorDelEntorno.LeerRegistroPorId(idTrabajoDeUsuario, true, true, true, aplicarJoin: false);
            var entorno = new EntornoDeTrabajo(gestorDelEntorno, trabajoDeUsuarioDtm);
            entorno.ProcesoIniciadoPorLaCola = iniciadoPorLaCola;

            entorno.PonerSemaforo();
            try
            {
                trabajoDeUsuarioDtm.Iniciado = DateTime.Now;
                trabajoDeUsuarioDtm.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.iniciado);
                trabajoDeUsuarioDtm = entorno.GestorDelEntorno.PersistirRegistro(trabajoDeUsuarioDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
            }
            catch (Exception e)
            {
                entorno.AnotarError(e);
                entorno.QuitarSemaforo("Iniciación cancelada");
                throw;
            }

            EjecutarTrabajo(entorno);
        }

        private static void EjecutarTrabajo(EntornoDeTrabajo entorno)
        {
            try
            {
                var metodo = GestorDeTrabajosSometido.ValidarExisteTrabajoSometido(entorno.TrabajoSometido);
                using (var contextoDelProceso = ContextoSe.ObtenerContexto(entorno.ContextoDelEntorno))
                {
                    entorno.contextoDelProceso = contextoDelProceso;
                    metodo.Invoke(null, new object[] { entorno });
                }
                entorno.TrabajoDeUsuario.Estado = !entorno.HayErrores
                    ? TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Terminado)
                    : TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.conErrores);
                entorno.ComunicarFinalizacion();
            }
            catch (Exception e)
            {
                entorno.TrabajoDeUsuario.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Error);
                if (e.InnerException != null)
                {
                    entorno.ComunicarError(e.InnerException);
                    throw e.InnerException;
                }

                entorno.ComunicarError(e);
                throw;
            }
            finally
            {
                entorno.TrabajoDeUsuario.Terminado = DateTime.Now;
                var parametros = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                parametros.Parametros[EnumParametro.accion] = EnumParametroTu.terminando;
                entorno.GestorDelEntorno.PersistirRegistro(entorno.TrabajoDeUsuario, parametros);
                entorno.QuitarSemaforo($"Trabajo finalizado: {(entorno.TrabajoDeUsuario.Estado == TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Terminado) ? "sin errores" : "con errores")}");
            }
        }

        public static void Bloquear(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var gestor = Gestor(contexto);
            var tuDtm = gestor.LeerRegistroPorId(idTrabajoDeUsuario, true, true, true, aplicarJoin: true);
            try
            {
                if (tuDtm.Estado != TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Pendiente))
                    throw new Exception($"El trabajo no se puede bloquear, ha de estar en estado pendiente y está en estado {TrabajoSometido.ToDto(tuDtm.Estado)}");
                tuDtm.Estado = TrabajoSometido.ToDtm(enumEstadosDeUnTrabajo.Bloqueado);
                gestor.PersistirRegistro(tuDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tuDtm, $"Trabajo bloqueado por el usuario {contexto.DatosDeConexion.Login}");
            }
            catch (Exception e)
            {
                GestorDeErroresDeUnTrabajo.AnotarError(contexto, tuDtm, e);
                GestorDeTrazasDeUnTrabajo.AnotarTraza(contexto, tuDtm, $"El usuario {contexto.DatosDeConexion.Login} no ha podido bloquear el trabajo");
                throw;
            }
        }

        public static void Desbloquear(ContextoSe contexto, int idTrabajoDeUsuario)
        {
            var gestor = Gestor(contexto);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario, true, true, true, aplicarJoin: true);
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
            var gestor = Gestor(contexto);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario, true, true, true, aplicarJoin: true);

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

        protected override IQueryable<TrabajoDeUsuarioDtm> AplicarFiltros(IQueryable<TrabajoDeUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.Equals(nameof(ElementoDtm.Nombre), StringComparison.CurrentCultureIgnoreCase))
                   registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, $"{nameof(TrabajoDeUsuarioDtm.Trabajo)}.{nameof(TrabajoDeUsuarioDtm.Trabajo.Nombre)}");

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

        protected override void AntesMapearRegistroParaInsertar(TrabajoDeUsuarioDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaInsertar(elemento, opciones);
            if (elemento.Estado.IsNullOrEmpty())
                elemento.Estado = enumEstadosDeUnTrabajo.Pendiente.ToDto();
            if (elemento.Parametros.IsNullOrEmpty())
                elemento.Parametros = "[]";
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
                    ParametrosJson.ValidarJson(registro.Parametros);
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
