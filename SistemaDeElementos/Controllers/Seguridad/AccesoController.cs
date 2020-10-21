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
            //_gestordeUsuarios.
            try
            {
                await HttpContext
                  .SignOutAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }
            return LocalRedirect("~/Acceso/Login.html");
        }
        public async Task<IActionResult> Login(string email, string password)
        {
            //_gestordeUsuarios.
            try
            {
                await HttpContext
                  .SignOutAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            var user = ("jjimenez", false);

            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.Name, user.Item1),
                  new Claim(ClaimTypes.Role, user.Item2 ? "admin" : "user"),
                  new Claim("email", email)
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

            return PanelDeControl();
        }
    }
}
