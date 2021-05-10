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
using ModeloDeDto.TrabajosSometido;
using GestoresDeNegocio.Entorno;
using Enumerados;
using ServicioDeCorreos;
using Utilidades;

namespace GestoresDeNegocio.TrabajosSometidos
{
    public class GestorDeCorreos : GestorDeElementos<ContextoSe, CorreoDtm, CorreoDto>
    {
        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<CorreoDtm, CorreoDto>();
                CreateMap<CorreoDto, CorreoDtm>();
            }
        }
        public GestorDeCorreos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }

        private static GestorDeCorreos Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCorreos(contexto, mapeador);
        }


        public static CorreoDtm CrearCorreoPara(ContextoSe contexto, List<string> receptores, string asunto, string cuerpo, List<string> elementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = GestorDeUsuarios.LeerUsuario(contexto, contexto.DatosDeConexion.IdUsuario).eMail;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = elementos.ToJson();
            correo.Archivos = archivos.ToJson();
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.PersistirRegistro(correo, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public static CorreoDtm CrearCorreoDe(ContextoSe contexto, string emisor, List<string> receptores, string asunto, string cuerpo, List<string> elementos, List<string> archivos)
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

        public static (int enviados, bool errores) EnviarCorreos(EntornoDeTrabajo entorno)
        {
            var gestor = Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var fltPendientes = new ClausulaDeFiltrado { Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Clausula = nameof(CorreoDtm.Enviado), Valor = null };
            var correosPendientes = gestor.LeerRegistros(0, -1, new List<ClausulaDeFiltrado> { fltPendientes }, null, null);
            var enviados = 0;
            var errores = false;
            foreach (var correoDtm in correosPendientes)
            {
                var tran = entorno.contextoDelProceso.IniciarTransaccion();
                try
                {
                    gestor.EnviarCorreoPara(correoDtm);
                    enviados++;
                    entorno.contextoDelProceso.Commit(tran);
                }
                catch (Exception e)
                {
                    errores = true;
                    entorno.contextoDelProceso.Rollback(tran);
                    entorno.AnotarError(e);
                }
            }
            return (enviados, errores);
        }

        public static void EnviarCorreoPara(ContextoSe contexto, List<string> receptores, string asunto, string cuerpo, List<string> elementos, List<string> archivos)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var correoDtm = CrearCorreoPara(contexto, receptores, asunto, cuerpo, elementos, archivos);
            gestor.EnviarCorreoPara(correoDtm);
        }

        public static void EnviarCorreoDe(ContextoSe contexto, string emisor, List<string> receptores, string asunto, string cuerpo, List<string> elementos, List<string> archivos)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var correoDtm = CrearCorreoDe(contexto, emisor, receptores, asunto, cuerpo, elementos, archivos);
            gestor.EnviarCorreoDe(correoDtm);
        }

        protected override void AntesDePersistir(CorreoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                registro.Creado = DateTime.Now;
            }
        }

        private void EnviarCorreoPara(CorreoDtm correoDtm)
        {
            var archivos = correoDtm.Archivos.JsonToLista<string>();
            var receptores = correoDtm.Receptores.JsonToLista<string>();
            string cuerpo = AdjuntarElementos(correoDtm);

            ServicioDeCorreo.EnviarCorreoPara(CacheDeVariable.ServidorDeCorreo, receptores, correoDtm.Asunto, cuerpo, true, archivos);
            correoDtm.Enviado = DateTime.Now;
            PersistirRegistro(correoDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
        }

        private void EnviarCorreoDe(CorreoDtm correoDtm)
        {
            var archivos = correoDtm.Archivos.JsonToLista<string>();
            var receptores = correoDtm.Receptores.JsonToLista<string>();
            string cuerpo = AdjuntarElementos(correoDtm);

            ServicioDeCorreo.EnviarCorreoDe(CacheDeVariable.ServidorDeCorreo, correoDtm.Emisor, receptores, correoDtm.Asunto, cuerpo, true, archivos);
            correoDtm.Enviado = DateTime.Now;
            PersistirRegistro(correoDtm, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
        }

        private static string AdjuntarElementos(CorreoDtm correoDtm)
        {
            var elementos = correoDtm.Elementos.JsonToLista<string>();
            var cuerpo = correoDtm.Cuerpo;
            foreach (string elemento in elementos)
            {
                cuerpo = $"{cuerpo}{Environment.NewLine}{Environment.NewLine}{elemento}";
            }

            return cuerpo;
        }
    }
}
