using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using System.Data;
using Newtonsoft.Json;

namespace GestoresDeNegocio.Negocio
{
    class GestorDeAuditorias
    {

        public static void RegistrarAuditoria<T>(ContextoSe contexto, enumTipoOperacion operacion, ElementoDtm elemento) where T: ElementoDtm
        {
            var json = JsonConvert.SerializeObject(elemento);
            var sentencia = $@"Insert into {GeneradorMd.EsquemaDeTabla(typeof(T))}.{GeneradorMd.NombreDeTabla(typeof(T))}_AUDITORIA (id_elemento, id_usuario, operacion, registro, auditado_el) 
                               values ({elemento.Id}
                                      ,{contexto.DatosDeConexion.IdUsuario}
                                      ,{operacion.ToBd()}
                                      ,{json}
                                      ,'{DateTime.Now}')";
            contexto.Database.ExecuteSqlRaw(sentencia);
        }

    }
}
