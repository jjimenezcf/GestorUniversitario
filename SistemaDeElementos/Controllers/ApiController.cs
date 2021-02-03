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


    }
}
