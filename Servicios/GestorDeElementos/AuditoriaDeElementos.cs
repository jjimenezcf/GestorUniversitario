using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Negocio;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Negocio;

namespace GestorDeElementos
{
    public class AuditoriaDeElementos
    {
        private string esquemaDeAuditoria { get; set; }
        private string tablaDeAuditoria { get; set; }
        public ContextoSe Contexto { get; }
        public class MapearAuditoria : Profile
        {
            public MapearAuditoria()
            {
                CreateMap<AuditoriaDtm, AuditoriaDto>();
                CreateMap<AuditoriaDto, AuditoriaDtm>();
            }
        }
        Type Dtm;
        Type Dto;
        public AuditoriaDeElementos(ContextoSe contexto, enumNegocio negocio)
        {
            Dtm = negocio.TipoDtm();
            Dto = negocio.TipoDto();
            tablaDeAuditoria = $"{GeneradorMd.NombreDeTabla(Dtm)}_AUDITORIA";
            esquemaDeAuditoria = GeneradorMd.EsquemaDeTabla(Dtm);
            Contexto = contexto;
        }


        public IEnumerable<AuditoriaDtm> LeerRegistros(int idElemento)
        {
            var consulta = new ConsultaSql<AuditoriaDtm>(Contexto.Traza,  
                 $@"
select ID as Id
     , ID_ELEMENTO as IdElemento
     , ID_USUARIO as IdUsuario
     , OPERACION as Operacion
     , REGISTRO as registroJson
     , AUDITADO_EL as AuditadoEl
from {esquemaDeAuditoria}.{tablaDeAuditoria} T1 WITH(NOLOCK)
where ID_ELEMENTO = {idElemento}");
            var registros = consulta.LanzarConsulta();
            return registros;
        }

        public static void RegistrarAuditoria(ContextoSe contexto, enumNegocio negocio, enumTipoOperacion operacion, IElementoDtm auditar)
        {
            auditar.UsuarioModificador = auditar.UsuarioCreador = null;
            var json = JsonConvert.SerializeObject(auditar);
            var valor = json
                .Replace("{", "")
                .Replace(",\"", Environment.NewLine)
                .Replace("\",", Environment.NewLine)
                .Replace("\"", "")
                .Replace("null,", Environment.NewLine)
                .Replace("}", "");

            var sentencia = $@"Insert into {GeneradorMd.EsquemaDeTabla(negocio.TipoDtm())}.{GeneradorMd.NombreDeTabla(negocio.TipoDtm())}_AUDITORIA (id_elemento, id_usuario, operacion, registro, auditado_el) 
                               values ({((ElementoDtm)auditar).Id}
                                      ,{contexto.DatosDeConexion.IdUsuario}
                                      ,'{operacion.ToBd()}'
                                      ,'{valor}'
                                      ,'{DateTime.Now}')";

            contexto.Database.ExecuteSqlRaw(sentencia);
        }
    }
}
