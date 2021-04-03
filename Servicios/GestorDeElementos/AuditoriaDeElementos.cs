using System;
using System.Collections.Generic;
using AutoMapper;
using Dapper;
using Gestor.Errores;
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

        public AuditoriaDtm LeerRegistroPorId(int id, bool emitirError = true)
        {
            var consulta = new ConsultaSql<AuditoriaDtm>(Contexto.Traza, Auditoria.sqlLeerPorId.Replace("[Esquema].[Tabla]", $"{esquemaDeAuditoria}.{tablaDeAuditoria}"));
            var restrictores = new Dictionary<string, object> { { "@Id", id } };
            var registros = consulta.LanzarConsulta(new DynamicParameters(restrictores));
            
            if (registros.Count == 0 && emitirError)
                GestorDeErrores.Emitir("No se ha localizado el registro pedido");

            return registros[0];
        }

        public IEnumerable<AuditoriaDtm> LeerRegistros(int idElemento, List<int> usuarios, int posicion, int cantidad)
        {
            var consulta = new ConsultaSql<AuditoriaDtm>(Contexto.Traza, Auditoria.sqlAuditoriaDeUnElemento.Replace("[Esquema].[Tabla]", $"{esquemaDeAuditoria}.{tablaDeAuditoria}"));

            consulta.AplicarClausulaIn(Auditoria.FiltroPorUsuario, Auditoria.AplicarFiltroPorUsuario, usuarios);

            var restrictores = new Dictionary<string, object> { { "@posicion", posicion }, { "@cantidad", cantidad }, {"@idElemento",idElemento } };
            var registros = consulta.LanzarConsulta(new DynamicParameters(restrictores));
            return registros;
        }

        public int ContarRegistros(int idElemento, List<int> usuarios)
        {
            var consulta = new ConsultaSql<RegistrosAfectados>(Contexto.Traza, Auditoria.sqlTotalAuditoria.Replace("Esquema.Tabla", $"{esquemaDeAuditoria}.{tablaDeAuditoria}"));

            consulta.AplicarClausulaIn(Auditoria.FiltroPorUsuario, Auditoria.AplicarFiltroPorUsuario, usuarios);

            var restrictor = new Dictionary<string, object> { { "@idElemento", idElemento } };
            var registros = consulta.LanzarConsulta(new DynamicParameters(restrictor));
            return registros[0].cantidad;

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
