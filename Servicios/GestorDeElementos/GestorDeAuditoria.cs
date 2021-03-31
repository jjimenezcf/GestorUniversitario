using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;

namespace GestorDeElementos
{
    class GestorDeAuditoria<T> where T : Registro
    {
        private string esquemaDeAuditoria { get; set; }
        private string tablaDeAuditoria { get; set; }
        public ContextoSe Contexto { get; }

        public GestorDeAuditoria(ContextoSe contexto)
        {
            tablaDeAuditoria = GeneradorMd.NombreDeTabla(typeof(T));
            esquemaDeAuditoria = GeneradorMd.EsquemaDeTabla(typeof(T));
            Contexto = contexto;
        }

        public IEnumerable<AuditoriaDtm> LeerRegistros(int idElemento)
        {
            var auditoria = Contexto.Auditoria
                .FromSqlInterpolated($@"select T1.ID, T1.ID_ELEMENTO, T1.ID_USUARIO, T2.LOGIN AS USUARIO, T1.OPERACION, T1.REGISTRO, T1.AUDITADO_EL
                 from {esquemaDeAuditoria}.{tablaDeAuditoria} T1 WITH(NOLOCK)
                 inner join entorno.usuario t2 on t2.id = t1.ID_USUARIO WITH(NOLOCK)
                 where ID_ELEMENTO = {idElemento}");
            return auditoria;
        }

        public static void RegistrarAuditoria(ContextoSe contexto, enumTipoOperacion operacion, IElementoDtm auditar)
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

            var sentencia = $@"Insert into {GeneradorMd.EsquemaDeTabla(typeof(T))}.{GeneradorMd.NombreDeTabla(typeof(T))}_AUDITORIA (id_elemento, id_usuario, operacion, registro, auditado_el) 
                               values ({((ElementoDtm)auditar).Id}
                                      ,{contexto.DatosDeConexion.IdUsuario}
                                      ,'{operacion.ToBd()}'
                                      ,'{valor}'
                                      ,'{DateTime.Now}')";

            contexto.Database.ExecuteSqlRaw(sentencia);
        }
    }
}
