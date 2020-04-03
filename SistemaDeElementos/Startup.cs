using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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

            services.AddRazorPages();
            var cadenaDeConexion = Configuracion.GetConnectionString(Literal.CadenaDeConexion);

            services.AddDbContext<CtoEntorno>(options => options.UseSqlServer(cadenaDeConexion));
            services.AddDbContext<CtoSeguridad>(options => options.UseSqlServer(cadenaDeConexion));

            services.AddScoped<Gestor.Errores.GestorDeErrores>();
            services.AddScoped<GestorDeUsuarios>();
            services.AddScoped<GestorDePermisos>();
            services.AddScoped<GestorDeVistasMvc>();
            services.AddScoped<GestorDeMenus>();

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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
