using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gestor.Errores;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;

namespace SistemaDeElementos.Controllers.Seguridad
{
    public class AccesoController : HomeController
    {
        GestorDeUsuarios _gestordeUsuarios;

        public AccesoController(ContextoSe contexto, GestorDeUsuarios gestorDeUsuarios, GestorDeErrores gestorDeErrores) : base(contexto, gestorDeErrores)
        {
            _gestordeUsuarios = gestorDeUsuarios;
        }

        public async Task<IActionResult> Logout()
        {
            await AnularLaCookie();
            return LocalRedirect("~/Acceso/Conectar.html");
        }

        private async Task AnularLaCookie()
        {
            try
            {
                await HttpContext
                  .SignOutAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }
        }

        [HttpPost]
        public async Task<IActionResult> Conectar(string email, string password)
        {
            await AnularLaCookie();
            var usuario = _gestordeUsuarios.Conectar(email, password);

            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.Email, usuario.Nombre),
                  new Claim(ClaimTypes.Name, usuario.NombreCompleto)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
                );
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value,
                ExpiresUtc = DateTime.Now.AddMinutes(8 * 60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return PanelDeControl(usuario);
        }
    }
}
