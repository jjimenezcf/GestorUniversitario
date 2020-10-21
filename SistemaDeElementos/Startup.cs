using System;
using AutoMapper;
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

            //services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = false;
            //});

            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            //    options.LoginPath = "/Acceso/Login.html";
            //    options.AccessDeniedPath = "/Acceso/Denegado.html";
            //    options.SlidingExpiration = true;
            //});


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AutenticacionSE";
                    options.LoginPath = "/Acceso/Login.html";
                    options.AccessDeniedPath = "/Acceso/Denegado.html";
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("SoloAdmin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });
            });

            services.AddScoped<Gestor.Errores.GestorDeErrores>();
            services.AddScoped<GestorDeArbolDeMenu>();
            services.AddScoped<GestorDeUsuarios>();
            services.AddScoped<GestorDePermisos>();
            services.AddScoped<GestorDeVistaMvc>();
            services.AddScoped<GestorDeMenus>();
            services.AddScoped<GestorDeVariables>();
            services.AddScoped<GestorDeVistaMvc>();
            services.AddScoped<GestorDePuestosDeTrabajo>();
            services.AddScoped<GestorDeClaseDePermisos>();
            services.AddScoped<GestorDePuestosDeUnUsuario>();
            services.AddScoped<GestorDeUsuariosDeUnPuesto>();
            services.AddScoped<GestorDeRolesDeUnPuesto>();
            services.AddScoped<GestorDePuestosDeUnRol>();
            services.AddScoped<GestorDeRoles>();
            services.AddScoped<GestorDePermisosDeUnRol>();
            services.AddScoped<GestorDeRolesDeUnPermiso>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
