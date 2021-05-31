using System;
using ServicioDeDatos;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestoresDeNegocio.Negocio;
using ServicioDeDatos.Negocio;
using ModeloDeDto.Negocio;
using GestorDeElementos;
using ModeloDeDto.Callejero;
using ServicioDeDatos.Callejero;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using ServicioDeDatos.Seguridad;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var servidorWeb = CreateWebHostBuilder(args).Build();
            InicializarBD(servidorWeb);
            servidorWeb.Run();
        }

        private static void InicializarBD(IWebHost sevidorWeb)
        {
            var scope = sevidorWeb.Services.CreateScope();
            var services = scope.ServiceProvider;
            InicializarNegocios(services);
        }

        private static void InicializarNegocios(IServiceProvider services)
        {
            var scope = services.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeNegocios>())
            {
                gestor.Contexto.IniciarTraza(nameof(InicializarNegocios));
                try
                {
                    gestor.Contexto.DatosDeConexion.CreandoModelo = true;
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Usuario, typeof(UsuarioDtm), typeof(UsuarioDto), "usuario.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.VistaMvc, typeof(VistaMvcDtm), typeof(VistaMvcDto), "vista.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Variable, typeof(VariableDtm), typeof(VariableDto), "cog-solid.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Menu, typeof(MenuDtm), typeof(MenuDto), "funcionalidad-3.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Puesto, typeof(PuestoDtm), typeof(PuestoDto), "puestoDeTrabajo.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Permiso, typeof(PermisoDtm), typeof(PermisoDto), "acceso.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Negocio, typeof(NegocioDtm), typeof(NegocioDto), "red.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Rol, typeof(RolDtm), typeof(RolDto), "roles.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Pais, typeof(PaisDtm), typeof(PaisDto), "paises_1.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Provincia, typeof(ProvinciaDtm), typeof(ProvinciaDto), "provincias_1.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Correo, typeof(CorreoDtm), typeof(CorreoDto), "Correo_1.svg");
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Municipio, typeof(MunicipioDtm), typeof(MunicipioDto), "cambio.svg");
                }
                finally
                {
                    gestor.Contexto.DatosDeConexion.CreandoModelo = false;
                }
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
