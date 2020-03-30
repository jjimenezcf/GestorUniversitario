using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Utilidades;

namespace Gestor.Elementos.Entorno
{
    public class ConstructorDelContexto : IDesignTimeDbContextFactory<CtoEntorno>
    {
        public CtoEntorno CreateDbContext(string[] arg)
        {
            var generador = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
            var configuaracion = generador.Build();
            var cadenaDeConexion = configuaracion.GetConnectionString(Gestor.Elementos.Literal.CadenaDeConexion);

            var opciones = new DbContextOptionsBuilder<CtoEntorno>();
            opciones.UseSqlServer(cadenaDeConexion);

            return new CtoEntorno(opciones.Options,configuaracion);
        }
    }


    public class GestorDeEntorno
    {

        public static string RenderMenuFuncional()
        {
            string[] parametros = {""};

            var contexto = new ConstructorDelContexto().CreateDbContext(parametros);
            contexto.IniciarTraza();


            var configuradorDeMapeos = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<MenuDto, MenuDtm>()
                   .ForMember(rm => rm.IdVistaMvc, em => em.MapFrom(s => s.VistaMvc != null ? s.VistaMvc.Id : int.Parse(null)))
                   .ForMember(rm => rm.IdPadre, em => em.MapFrom(m => m.Padre != null ? m.Padre.Id : int.Parse(null)));

                cfg.CreateMap<MenuDtm, MenuDto>()
                   .ForMember(dtm => dtm.Submenus, dto => dto.MapFrom(dtm => (List<MenuDto>)null))
                   .ForMember(dtm => dtm.VistaMvc, dto => dto.MapFrom(dtm => (VistaMvcDto)null))
                   .ForMember(dtm => dtm.Padre, dto => dto.MapFrom(dtm => dtm.Padre));
            });


            IMapper mapeador = configuradorDeMapeos.CreateMapper();

            var gestorDeMenus = new GestorDeMenus(contexto, mapeador);

            List<MenuDto> menu = gestorDeMenus.LeerMenuSe();

            var menuHtml = @$"<ul id='id_menuraiz' class=¨menu-contenido¨>{Environment.NewLine}" +
                           @$"   {RenderOpcionesMenu(menu, 0)}{Environment.NewLine}" +
                           @$"</ul>{Environment.NewLine}";
            return menuHtml.Replace("¨", "\"");
        }

        private static string RenderOpcionesMenu(List<MenuDto> opcionesMenu, int idMenuPadre)
        {
            var menuHtml = "";
            foreach (MenuDto fDto in opcionesMenu)
            {
                menuHtml = menuHtml + RenderMenu(funcion: fDto, idMenuPadre);
            }
            return menuHtml;
        }

        private static string RenderMenu(MenuDto funcion, int idMenuPadre)
        {
            if (funcion.VistaMvc != null)
            {
                var opcionHtml = RenderAccionMenu(accion: funcion.VistaMvc);
                return opcionHtml;
            }

            var subMenuHtml = funcion.Submenus != null ? RenderOpcionesMenu(funcion.Submenus, funcion.Id) : "";

            var idMenuHtml = $"id_menu_{funcion.Id}";
            var idMenuPadreHtml = $"id_menu_{idMenuPadre}";
            var liHtml =
                $@"<li>{Environment.NewLine}" +
                $@"  <a>{Environment.NewLine}" +
                $@"     {ComponerMenu(literalOpcion: funcion.Nombre, icono: funcion.Icono, idMenu: idMenuHtml)}" +
                $@"  </a>{Environment.NewLine}" +
                $@"  <ul id=¨{idMenuHtml}¨ name=¨menu¨ menu-padre=¨{idMenuPadreHtml}¨ menu-plegado=¨true¨>{Environment.NewLine}" +
                      subMenuHtml +
                $@"  </ul>{Environment.NewLine}" +
                $@"</li>{Environment.NewLine}";

            return liHtml;
        }

        private static string RenderAccionMenu(VistaMvcDto accion)
        {
            var idHtml = $"{accion.Id}";
            var opcionHtml =
            $@"<li>{Environment.NewLine}" +
            $@"  <input id='{idHtml}' type='button' class='menu-opcion' value='{accion.Nombre}' onclick=¨Menu.OpcionSeleccionada('{idHtml}','{accion.Controlador}','{accion.Accion}')¨ />{Environment.NewLine}" +
            $@"</li>{Environment.NewLine}";

            return opcionHtml;
        }

        private static object ComponerMenu(string literalOpcion, string icono, string idMenu)
        {
            var opcionHtml = "";

            if (!icono.IsNullOrEmpty() && File.Exists(@$"wwwroot\images\menu\{icono}"))
                opcionHtml = @$"<img src=¨/images/menu/{icono}¨ class=¨icono izquierdo¨ />{Environment.NewLine}";

            opcionHtml = $@"{opcionHtml}{literalOpcion}{Environment.NewLine}";

            if (!idMenu.IsNullOrEmpty())
                opcionHtml = $"{opcionHtml}<img src=¨/images/menu/angle-down-solid.svg¨ class=¨icono derecho¨ onclick=¨Menu.MenuPulsado('{idMenu}')¨/>{Environment.NewLine}";

            return opcionHtml;
        }

    }
}

