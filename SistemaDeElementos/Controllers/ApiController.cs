using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gestor.Errores;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Http;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using Microsoft.AspNetCore.Mvc;
using ModeloDeDto;
using Utilidades;
using System.Reflection;
using GestorDeElementos;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public static class ApiController
    {

        public static void CumplimentarDatosDeUsuarioDeConexion(ContextoSe contexto, IMapper mapeador, HttpContext httpContext)
        {
            contexto.DatosDeConexion.Login = ApiController.ObtenerUsuarioDeLaRequest(httpContext);
            var gestorDeUsuario = GestorDeUsuarios.Gestor(contexto, mapeador);
            var usuario = gestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), contexto.DatosDeConexion.Login);
            contexto.DatosDeConexion.IdUsuario = usuario.Id;
            contexto.DatosDeConexion.EsAdministrador = usuario.EsAdministrador;
            contexto.Mapeador = mapeador;
        }

        public static string ObtenerUsuarioDeLaRequest(HttpContext httpContext)
        {
            if (httpContext == null)
                return null;

            if (httpContext.User == null)
                GestorDeErrores.Emitir("Conexión no establecidad");

            var caracter = httpContext.User.FindFirst(nameof(UsuarioDto.Login));
            if (caracter == null)
                GestorDeErrores.Emitir("Usuario no definido");

            return httpContext.User.FindFirst(nameof(UsuarioDto.Login)).Value;
        }

        public static (IEnumerable<T> elementos, int total) LeerDatosParaElGrid<T>(
            Func<IEnumerable<T>> Leer
          , Func<int> Contar)
        where T : ElementoDto
        {
            int total;
            IEnumerable<T> elementos;
            var opcionesDeMapeo = new Dictionary<string, object>();
            opcionesDeMapeo.Add(ElementoDto.DescargarGestionDocumental, false);
            elementos = Leer();
            total = Contar();
            return (elementos, total);
        }

        public static List<Dictionary<string, object>> ElementosLeidos<T>(ContextoSe contexto, List<T> elementos, Func<enumModoDeAccesoDeDatos> LeerModoAccesoAlElemento)
        where T : ElementoDto
        {
            var listaDeElementos = new List<Dictionary<string, object>>();
            if (elementos.Count > 0)
            {
                PropertyInfo[] propiedades = elementos[0].GetType().GetProperties();

                foreach (T elemento in elementos)
                {
                    var registro = new Dictionary<string, object>();
                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        object valor = elemento.GetType().GetProperty(propiedad.Name).GetValue(elemento);
                        registro[propiedad.Name] = valor == null ? "" : valor;
                    }
                    var ma = LeerModoAccesoAlElemento();
                    registro[nameof(Resultado.ModoDeAcceso)] = ma.Render();
                    listaDeElementos.Add(registro);
                }
            }

            return listaDeElementos;
        }

        public static void PrepararError(Exception e, Resultado r,  string asunto)
        {
            r.Estado = enumEstadoPeticion.Error;
            r.consola = GestorDeErrores.Detalle(e);
            if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                r.Mensaje = e.Message;
            else
                r.Mensaje = $"{asunto} {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
        }
    }

}
