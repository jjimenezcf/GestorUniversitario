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

namespace GestoresDeNegocio.TrabajosSometidos
{
    public class GestorDeCorreos : GestorDeElementos<ContextoSe, CorreoDtm, CorreoDto>
    {
        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<CorreoDtm, CorreoDto>()
                .ForMember(dto => dto.Creador, dtm => dtm.MapFrom(x => $"({x.Usuario.Login})- {x.Usuario.Nombre} {x.Usuario.Apellido}"));
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


        public static CorreoDtm CrearCorreoPara(ContextoSe contexto, List<string> receptores, string asunto, string cuerpo, List<string> urlsDeElementos, List<string> archivos)
        {
            var correo = new CorreoDtm();
            correo.IdUsuario = contexto.DatosDeConexion.IdUsuario;
            correo.Emisor = new ServicioDeCorreo(CacheDeVariable.ServidorDeCorreo).Emisor;
            correo.Receptores = receptores.ToJson();
            correo.Asunto = asunto;
            correo.Cuerpo = cuerpo;
            correo.Elementos = urlsDeElementos.ToJson();
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

        internal static void EnviarCorreoPendientes(ContextoSe contexto)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtro = new ClausulaDeFiltrado(nameof(CorreoDtm.Enviado), CriteriosDeFiltrado.esNulo);
            var pendientes = gestor.LeerRegistros(0, -1, new List<ClausulaDeFiltrado> { filtro });
            foreach (var pendiente in pendientes)
                try
                {
                    gestor.EnviarCorreoDe(pendiente);
                }
                catch (Exception e)
                {
                    try
                    {
                        ServicioDeCorreo.EnviarCorreoPara(CacheDeVariable.ServidorDeCorreo
                            , new List<string> { "juan.jimenez@gmail.com" }
                            , "Fallo al enviar cooreos"
                            , $"Error al enviar el correo con id  {pendiente.Id}{Environment.NewLine}{GestorDeErrores.Mensaje(e)}"
                            );
                        pendiente.Enviado = DateTime.Now;
                        gestor.PersistirRegistro(pendiente, new ParametrosDeNegocio(enumTipoOperacion.Modificar));
                    }
                    catch(Exception ei)
                    {
                        gestor.Contexto.AnotarExcepcion(ei);
                    }
                }
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
            registros = registros.Include(p => p.Usuario);
            return registros;
        }

        protected override IQueryable<CorreoDtm> AplicarFiltros(IQueryable<CorreoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(ElementoDtm.Nombre).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        registros = registros.Where(x => x.Asunto.Contains(filtro.Valor));
                    else
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Asunto.Equals(filtro.Valor));
                    else
                        GestorDeErrores.Emitir($"Se ha solicitado filtrar por {filtro.Criterio} en el gestor {nameof(GestorDeTrabajosDeUsuario)} y no se ha implementado el filtro");
                }
            }

            return registros;
        }
    }
}
