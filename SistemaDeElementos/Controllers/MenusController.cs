using System;
using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Utilidades;

namespace MVCSistemaDeElementos.Controllers
{
    public class MenusController : EntidadController<CtoEntorno, MenuDtm, MenuDto>
    {
        public GestorDeMenus GestorDeMenus { get; set; }

        public MenusController(GestorDeMenus gestorDeMenus, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeMenus,
          gestorDeErrores,
          new CrudMenus(ModoDescriptor.Consulta)
        )
        {
            GestorDeMenus = gestorDeMenus;
        }

        public string RenderMenu(string usuario)
        {
            List<MenuDto> menu = GestorDeMenus.LeerMenuSe();

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

            if (!icono.IsNullOrEmpty() ) //&& File.Exists(@$"wwwroot\images\menu\{icono}"))
                opcionHtml = @$"<img src=¨/images/menu/{icono}¨ class=¨icono izquierdo¨ />{Environment.NewLine}";

            opcionHtml = $@"{opcionHtml}{literalOpcion}{Environment.NewLine}";

            if (!idMenu.IsNullOrEmpty())
                opcionHtml = $"{opcionHtml}<img src=¨/images/menu/angle-down-solid.svg¨ class=¨icono derecho¨ onclick=¨Menu.MenuPulsado('{idMenu}')¨/>{Environment.NewLine}";

            return opcionHtml;
        }
    }
}