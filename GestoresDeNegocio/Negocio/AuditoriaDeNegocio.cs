using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorDeElementos;
using ModeloDeDto.Negocio;
using ServicioDeDatos;
using ServicioDeDatos.Negocio;

namespace GestoresDeNegocio.Negocio
{
    public class AuditoriaDeNegocio
    {
        public static IEnumerable<AuditoriaDto> LeerElementos(ContextoSe contexto, enumNegocio negocio, int idElemento, List<int> usuarios, int posicion, int cantidad)
        {
            var a = new AuditoriaDeElementos(contexto, negocio);
            var registros = a.LeerRegistros(idElemento, usuarios, posicion, cantidad);
            var elementos = new List<AuditoriaDto>();

            foreach (var registro in registros)
            {
                var elemento = MapearRegistro(contexto, registro);
                elementos.Add(elemento);
            }
            return elementos;
        }


        public static int ContarElementos(ContextoSe contexto, enumNegocio negocio, int idElemento, List<int> usuarios)
        {
            var a = new AuditoriaDeElementos(contexto, negocio);
            var cantidad = a.ContarRegistros(idElemento, usuarios);
            return cantidad;
        }

        public static AuditoriaDto LeerElemento(ContextoSe contexto, enumNegocio negocio, int id)
        {
            var a = new AuditoriaDeElementos(contexto, negocio);
            var registro = a.LeerRegistroPorId(id);
            var elemento = MapearRegistro(contexto, registro);
            return elemento;
        }
        private static AuditoriaDto MapearRegistro(ContextoSe contexto, AuditoriaDtm registro)
        {
            var elemento = new AuditoriaDto();
            elemento.Id = registro.Id;
            elemento.IdElemento = registro.IdElemento;
            elemento.IdUsuario = registro.IdUsuario;
            elemento.AuditadoEl = registro.AuditadoEl;
            elemento.Operacion = registro.Operacion;
            elemento.registroJson = registro.registroJson;
            elemento.Usuario = Entorno.GestorDeUsuarios.LeerUsuario(contexto, elemento.IdUsuario);
            return elemento;
        }
    }
}
