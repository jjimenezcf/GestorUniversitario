using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using Newtonsoft.Json;
using ServicioDeDatos;
using static Gestor.Errores.GestorDeErrores;

namespace SistemaDeElementos.Controllers.Seguridad
{
    public class AccesoController : HomeController
    {
        private readonly ILogger<AccesoController> _logger;
        GestorDeUsuarios _gestordeUsuarios;


        public AccesoController(ILogger<AccesoController> logger, ContextoSe contexto, GestorDeUsuarios gestorDeUsuarios, GestorDeErrores gestorDeErrores) 
        : base(contexto, gestorDeUsuarios.Mapeador, gestorDeErrores)
        {
            _logger = logger;
            _gestordeUsuarios = gestorDeUsuarios;

        }

        public async Task<IActionResult> Logout()
        {
            await AnularLaCookie();
            return LocalRedirect("~/Acceso/Conectar.html");
        }


        //END-POINT: Desde Conectar.ts
        public JsonResult epReferenciarFoto(string restrictor)
        {
            var r = new Resultado();

            try
            {

                List<ClausulaDeFiltrado> filtros = JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(restrictor);
                var opcionesDeMapeo = new Dictionary<string, object>();
                opcionesDeMapeo.Add(ltrParametrosDto.DescargarGestionDocumental, true);

                var elementos = _gestordeUsuarios.LeerElementos(0, -1, filtros, null, opcionesDeMapeo).ToList();

                if (elementos.Count == 0)
                    Emitir($"No se ha localizado el usuario: {filtros[0].Valor}");

                if (elementos.Count > 1)
                    throw new Exception($"Hay más de un usuario identificado como: {filtros[0].Valor}");

                r.Datos = elementos[0].Foto;
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = $"se han leido 1 {(1 > 1 ? "registros" : "registro")}";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = Detalle(e);

                if (e.Data.Contains(Datos.Mostrar) && (bool)e.Data[Datos.Mostrar])
                    r.Mensaje = e.Message;
                else
                    r.Mensaje = "Error al leer";
            }

            return new JsonResult(r);

        }


        //END-POINT: Desde Conectar.ts
        [HttpPost]
        public JsonResult epValidarAcceso(string login, string password)
        {
            var r = new Resultado();

            try
            {
                _gestordeUsuarios.ValidarUsuario(login, password);
                r.Datos = null;
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = $"usuario validado";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = Detalle(e);
                r.Mensaje = "Error al validar usuario";
            }
            return new JsonResult(r);
        }



        [HttpPost]
        public async Task<IActionResult> Conectar(string login, string password)
        {
            await AnularLaCookie();

            UsuarioDto usuario;
            try
            {
                usuario = _gestordeUsuarios.ValidarUsuario(login, password);
                await registrarLaCookie(usuario);
            }
            catch
            {
                return await Logout();
            }

            return PanelDeControl(usuario);
        }

        private async Task registrarLaCookie(UsuarioDto usuario)
        {
            var caracteres = new List<Claim>
                {
                      new Claim(nameof(UsuarioDto.Id), usuario.Id.ToString()),
                      new Claim(nameof(UsuarioDto.Login), usuario.Login),
                      new Claim(ClaimTypes.Name, usuario.NombreCompleto)
                };

            var caracteresIdentitarios = new ClaimsIdentity(caracteres, CookieAuthenticationDefaults.AuthenticationScheme);
            var propiedadesDeAutentificacion = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value,
                ExpiresUtc = DateTime.Now.AddMinutes(8 * 60)
            };

            HttpContext.User = new ClaimsPrincipal(caracteresIdentitarios);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User, propiedadesDeAutentificacion);
        }

        private async Task AnularLaCookie()
        {
            try
            {
                await HttpContext
                  .SignOutAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, $"no había conexión");
            }
        }


    }
}
