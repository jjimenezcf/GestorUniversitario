using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorDeElementos;
using ModeloDeDto.Negocio;
using ServicioDeDatos;

namespace GestoresDeNegocio.Negocio
{
   public class AuditoriaDeNegocio
    {
        public static IEnumerable<AuditoriaDto> LeerElementos(ContextoSe contexto, enumNegocio negocio, int idElemento, int posicion, int cantidad)
        {
            var a = new AuditoriaDeElementos(contexto, negocio);
            var registros = a.LeerRegistros(idElemento, posicion, cantidad);
            var elementos = new List<AuditoriaDto>();

            foreach (var registro in registros)
            {
                var elemento = new AuditoriaDto();
                elemento.Id = registro.Id;
                elemento.IdElemento = registro.IdElemento;
                elemento.IdUsuario = registro.IdUsuario;
                elemento.AuditadoEl = registro.AuditadoEl;
                elemento.Operacion = registro.Operacion;
                elemento.registroJson = registro.registroJson;
                elemento.Usuario = Entorno.GestorDeUsuarios.LeerUsuario(contexto, elemento.IdUsuario);
                elementos.Add(elemento);
            }
            return elementos;
        }

        public static int ContarElementos(ContextoSe contexto, enumNegocio negocio, int idElemento)
        {
            var a = new AuditoriaDeElementos(contexto, negocio);
            var cantidad = a.ContarRegistros(idElemento);
            return cantidad;
        }
    }
}
