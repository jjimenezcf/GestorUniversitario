using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using System;
using GestoresDeNegocio.Entorno;
using Enumerados;
using ServicioDeCorreos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ModeloDeDto;
using Gestor.Errores;
using Utilidades;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Entorno;
using GestoresDeNegocio.Negocio;
using System.ComponentModel;
using static ServicioDeCorreos.ServicioDeCorreo;
using System.Threading.Tasks;
using System.Threading;
using Dapper;

namespace GestoresDeNegocio.TrabajosSometidos
{
    public class GestorDeCorreos : GestorDeElementos<ContextoSe, CorreoDtm, CorreoDto>
    {

        public class ltrParamCorreos
        {
            internal static readonly string usuarios = nameof(usuarios);
            internal static readonly string puestos = nameof(puestos);
            internal static readonly string receptores = nameof(receptores);
            internal static readonly string asunto = nameof(asunto);
            internal static readonly string cuerpo = nameof(cuerpo);
            internal static readonly string adjuntos = nameof(adjuntos);
            internal static readonly string archivos = nameof(archivos);
            internal static readonly string elementosDeNegocio = nameof(elementosDeNegocio);
            internal static readonly string JoinConUsuarios = nameof(JoinConUsuarios);
        }


        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<CorreoDtm, CorreoDto>()
                .ForMember(dto => dto.Creador, dtm => dtm.MapFrom(x => $"({x.Usuario.Login})- {x.Usuario.Nombre} {x.Usuario.Apellido}"));
                CreateMap<CorreoDto, CorreoDtm>();
            }
        }

        public static bool PermiteElEnvioDeCorreo<T>() where T : ElementoDto
        {
            try
            {
                ExtensionesDto.UrlParaMostrarUnDto(typeof(T));
            }
            catch
            {
               return false;
            }
            return true;
        }

        public GestorDeCorreos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }

        public static GestorDeCorreos Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCorreos(contexto, mapeador);
        }


        public static CorreoDtm CrearCorreoPara(ContextoSe contexto, List<string> receptores, string asunto, string cuerpo, List<TipoDtoElmento> elementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = new ServicioDeCorreo(CacheDeVariable.Cfg_ServidorDeCorreo).Emisor;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = elementos.ToJson();
            correo.Archivos = archivos.ToJson();
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.PersistirRegistro(correo, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public static CorreoDtm CrearCorreoDe(ContextoSe contexto, string emisor, List<string> receptores, string asunto, string cuerpo, List<TipoDtoElmento> elementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = emisor;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = elementos.ToJson();
            correo.Archivos = archivos.ToJson();
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.PersistirRegistro(correo, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public static CorreoDtm CrearCorreoDe(ContextoSe contexto, string parametrosJson)
        {
            Dictionary<string, object> parametros = parametrosJson.ToDiccionarioDeParametros();

            if (!parametros.ContainsKey(ltrParamCorreos.archivos))
                parametros[ltrParamCorreos.archivos] = new List<string>();

            ValidarParametrosDeCorreo(contexto, parametros);

            return CrearCorreoDe(contexto
                , GestorDeUsuarios.LeerUsuario(contexto, contexto.DatosDeConexion.IdUsuario).eMail
                , (List<string>)parametros[ltrParamCorreos.receptores]
                , (string)parametros[ltrParamCorreos.asunto]
                , (string)parametros[ltrParamCorreos.cuerpo]
                , (List<TipoDtoElmento>)parametros[ltrParamCorreos.adjuntos]
                , (List<string>)parametros[ltrParamCorreos.archivos]);
        }


        private static void ValidarParametrosDeCorreo(ContextoSe contexto, Dictionary<string, object> parametros)
        {
            if (!parametros.ContainsKey(ltrParamCorreos.usuarios) && !parametros.ContainsKey(ltrParamCorreos.puestos))
                GestorDeErrores.Emitir("Debe indicar algún receptor");

            var usuarios = parametros.ContainsKey(ltrParamCorreos.usuarios) ? parametros[ltrParamCorreos.usuarios].ToString().JsonToLista<int>() : new List<int>();
            var puestos = parametros.ContainsKey(ltrParamCorreos.puestos) ? parametros[ltrParamCorreos.puestos].ToString().JsonToLista<int>() : new List<int>();

            if (usuarios.Count == 0 && puestos.Count == 0)
                GestorDeErrores.Emitir("Debe indicar algún receptor");

            var receptores = "";
            foreach (var idUsuario in usuarios)
                receptores = $"{receptores};{GestorDeUsuarios.LeerUsuario(contexto, idUsuario).eMail}";
            foreach (var idPuesto in puestos)
            {
                List<UsuarioDtm> usuariosDeUnPuesto = GestorDePuestosDeTrabajo.LeerUsuarios(contexto, idPuesto);
                foreach (var usuario in usuariosDeUnPuesto)
                    receptores = $"{receptores};{GestorDeUsuarios.LeerUsuario(contexto, usuario.Id).eMail}";
            }

            parametros[ltrParamCorreos.receptores] = receptores.Substring(1).ToLista<string>();

            if (((string)parametros[ltrParamCorreos.asunto]).IsNullOrEmpty()) GestorDeErrores.Emitir("Debe indicar el asunto");
            if (((string)parametros[ltrParamCorreos.cuerpo]).IsNullOrEmpty()) GestorDeErrores.Emitir("Debe indicar el cuerpo");

            if (parametros.ContainsKey(ltrParamCorreos.adjuntos))
            {
                //var lista = new List<TipoDtoElmento>();
                var elementosDto = parametros[ltrParamCorreos.adjuntos].ToString().JsonToLista<TipoDtoElmento>();
                //foreach (var elementoDto in elementosDto)
                //{
                //    var partes = elementoDto.Split("#:#");

                //    if (partes.Length != 3)
                //        GestorDeErrores.Emitir($"Intenta enviar un correo adjuntando un elemento mal definido. debe indicar el tipoDto, el IdElemento y la referencia, sólo se han indicado {partes.Length} parámetros");


                //    if(partes[0].IsNullOrEmpty() || partes[2].IsNullOrEmpty() || partes[1].Entero() == 0)
                //    {
                //        GestorDeErrores.Emitir($"Intenta enviar un correo adjuntando un elemento mal definido. debe indicar el tipoDto, el IdElemento y la referencia, el sistema ha indicado {partes[0]}:{partes[1]}:{partes[2]}");
                //    }

                //    var elemento = new TipoDtoElmento { TipoDto = partes[0], IdElemento = partes[1].Entero(), Referencia = partes[2]};
                //    lista.Add(GestorDeNegocios.ValidarElementoDto(elemento));
                // }

                parametros[ltrParamCorreos.adjuntos] = elementosDto; // lista;
            }

            //TODO: Validar que las rutas de los archivos o los Ids de los archivos existen

        }

        private static string AdjuntarElementos(CorreoDtm correoDtm)
        {
            var elementos = correoDtm.Elementos.JsonToLista<TipoDtoElmento>();
            var cuerpo = correoDtm.Cuerpo;
            foreach (TipoDtoElmento elemento in elementos)
            {
                cuerpo = $"{cuerpo}{Environment.NewLine}{GestorDeNegocios.ComponerUrl(elemento)}";
            }

            return cuerpo;
        }


        internal void EnviarCorreoPendientesAsync()
        {
            var filtro = new ClausulaDeFiltrado(nameof(CorreoDtm.Enviado), CriteriosDeFiltrado.esNulo);
            var parametros = new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo);
            parametros.Parametros[ltrParamCorreos.JoinConUsuarios] = false;
            var pendientes = LeerRegistros(0, -1, new List<ClausulaDeFiltrado> { filtro }, null, null, parametros);
            foreach (var pendiente in pendientes)
                try
                {
                    EnviarCorreoDeAsync(pendiente); 
                }
                catch (Exception e)
                {
                    try
                    {
                        ServicioDeCorreo.EnviarCorreoPara(CacheDeVariable.Cfg_ServidorDeCorreo
                            , new List<string> { "juan.jimenez@gmail.com" }
                            , "Fallo al enviar cooreos"
                            , $"Error al enviar el correo con id  {pendiente.Id}{Environment.NewLine}{GestorDeErrores.Mensaje(e)}"
                            );
                        pendiente.Enviado = DateTime.Now;
                        PersistirRegistro(pendiente, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                    }
                    catch (Exception ei)
                    {
                        Contexto.AnotarExcepcion(ei);
                    }
                }
        }

        private Task EnviarCorreoDeAsync(CorreoDtm correoDtm)
        {
            var archivos = correoDtm.Archivos.JsonToLista<string>();
            var receptores = correoDtm.Receptores.JsonToLista<string>();
            string cuerpo = AdjuntarElementos(correoDtm);

            var manejador = new ManejadorDeCorreo
            {
                CorreoDtm = correoDtm,
                Contexto = ContextoSe.ObtenerContexto(Contexto),
                GestorDeCorreo = typeof(GestorDeCorreos)
            };

            EnviarCorreoDe(CacheDeVariable.Cfg_ServidorDeCorreo, correoDtm.Emisor, receptores, correoDtm.Asunto, cuerpo, true, archivos, manejador);

            return Task.FromResult(new ResultadoDelProceso(true));

        }

        public static void ActualizarFechaDeEnvio(CorreoDtm correoDtm)
        {
            var sentencia = new ConsultaSql<CorreoDtm>(CorreoSql.ActualizarFechaDeEnvio, CacheDeVariable.Cfg_HayQueDebuggar, $"{nameof(ActualizarFechaDeEnvio)}");
            var valores = new Dictionary<string, object> { { $"@{nameof(CorreoDtm.Enviado)}", DateTime.Now }, { $"@{nameof(CorreoDtm.Id)}", correoDtm.Id } };
            sentencia.EjecutarSentencia(new DynamicParameters(valores));
        }

        // Método invocados por reflexión desde el servidor de correos
        public static void IndicarQueElCorreoHaSidoEnviado(ContextoSe contexto,  CorreoDtm correoDtm)
        {
            correoDtm.Enviado = DateTime.Now;
            var gestor = Gestor(contexto, contexto.Mapeador);
            gestor.PersistirRegistro(correoDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
            ActualizarFechaDeEnvio(correoDtm);
        }

        // Método invocados por reflexión desde el servidor de correos
        public static void AnotarTraza(ContextoSe contexto, CorreoDtm correoDtm)
        {
            contexto.IniciarTraza("EnviosDeCorreo_Cancelacion");
            contexto.Traza.AnotarMensaje("Correo enviado", $"El correo {correoDtm.Asunto} se ha cancelado");
            contexto.CerrarTraza("Fin de anotación de cancelación");
        }

        // Método invocados por reflexión desde el servidor de correos
        public static void AnotarExcepcion(ContextoSe contexto, CorreoDtm correoDtm, Exception error)
        {
            contexto.IniciarTraza("EnviosDeCorreo_Error");
            contexto.Traza.AnotarMensaje("Error en el envío", $"El correo {correoDtm.Asunto} NO se ha podido enviar.{Environment.NewLine}Error {error.Message}");
            contexto.CerrarTraza("Fin de anotación de error");
        }

        protected override void AntesDePersistir(CorreoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                registro.Creado = DateTime.Now;
            }
        }

        protected override IQueryable<CorreoDtm> AplicarJoins(IQueryable<CorreoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            if (HacerJoinCon(parametros,ltrParamCorreos.JoinConUsuarios))
                registros = registros.Include(p => p.Usuario);
            return registros;
        }

        protected override IQueryable<CorreoDtm> AplicarFiltros(IQueryable<CorreoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(ElementoDtm.Nombre).ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(CorreoDtm.Asunto));

                if (filtro.Clausula.ToLower() == ltrFltCorreosDto.seHaEnviado.ToLower())
                    registros = Filtrar.AplicarFiltroPorFechaNoNula(registros, nameof(CorreoDtm.Enviado));

                if (filtro.Clausula.ToLower() == ltrFltCorreosDto.NoSeHaEnviado.ToLower())
                    registros = Filtrar.AplicarFiltroPorFechaNula(registros, nameof(CorreoDtm.Enviado));

            }

            return registros;
        }
    }
}
