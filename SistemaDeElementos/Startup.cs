using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicioDeDatos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Seguridad;
using ServicioDeDatos.Seguridad;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AutoMapper;
using System;
using GestoresDeNegocio.Negocio;
using GestoresDeNegocio.Callejero;
using GestoresDeNegocio.TrabajosSometidos;
using ColaDeTrabajosSometidos;
using Gestor.Errores;

namespace MVCSistemaDeElementos
{
    public class Startup
    {
        public IConfiguration Configuracion { get; }

        public Startup(IConfiguration configuracion)
        {
            Configuracion = configuracion;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddIdentity<UsuarioDtm, RolDtm>().AddDefaultTokenProviders();


            // Identity Services
            //services.AddTransient<IUserStore<UsuarioDtm>, GestorDeUsuarios>();
            //services.AddTransient<IRoleStore<RolDtm>, GestorDeRoles>();

            services.AddRazorPages();

            var cadenaDeConexion = Configuracion.GetConnectionString(Literal.CadenaDeConexion);

            services.AddDbContext<ContextoSe>(options => options.UseSqlServer(cadenaDeConexion));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AutenticacionSE";
                    options.LoginPath = "/Acceso/Conectar.html";
                    options.AccessDeniedPath = "/Acceso/Denegado.html";
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("SoloAdmin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });
            });

            services.AddScoped<GestorDeErrores>();
            services.AddScoped<GestorDeArbolDeMenu>();
            services.AddScoped<GestorDeVistaMvc>();
            services.AddScoped<GestorDeMenus>();
            services.AddScoped<GestorDeVariables>();

            services.AddScoped<GestorDeUsuarios>();
            services.AddScoped<GestorDePermisos>();
            services.AddScoped<GestorDePuestosDeTrabajo>();
            services.AddScoped<GestorDeClaseDePermisos>();
            services.AddScoped<GestorDePuestosDeUnUsuario>();
            services.AddScoped<GestorDeUsuariosDeUnPuesto>();
            services.AddScoped<GestorDeRolesDeUnPuesto>();
            services.AddScoped<GestorDePuestosDeUnRol>();
            services.AddScoped<GestorDeRoles>();
            services.AddScoped<GestorDePermisosDeUnRol>();
            services.AddScoped<GestorDeRolesDeUnPermiso>();
            services.AddScoped<GestorDePermisosDeUnUsuario>();
            services.AddScoped<GestorDePermisosDeUnPuesto>();

            services.AddScoped<GestorDeNegocios>();
            services.AddScoped<GestorDeParametrosDeNegocio>();

            services.AddScoped<GestorDePaises>();
            services.AddScoped<GestorDeProvincias>();
            services.AddScoped<GestorDeMunicipios>();
            services.AddScoped<GestorDeTiposDeVia>();
            services.AddScoped<GestorDeCodigosPostales>();

            services.AddScoped<GestorDeCpsDeUnaProvincia>();
            

            services.AddScoped<GestorDeTrabajosSometido>();
            services.AddScoped<GestorDeTrabajosDeUsuario>();
            services.AddScoped<GestorDeTrazasDeUnTrabajo>();
            services.AddScoped<GestorDeErroresDeUnTrabajo>();
            services.AddScoped<GestorDeCorreos>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddHostedService<BackgroundCola>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
