using System;
using System.Collections.Generic;
using System.Text;
using Utilidades;

namespace Gestor.Elementos.Entorno
{
    public class GestorDeEntorno
    {

        public static string RenderMenuFuncional()
        {
            var menu = GestorDeFuncionalidad.MenuPrincipal();

            var menuHtml = @$"<ul id='id_menuraiz' class=¨menu-contenido¨>{Environment.NewLine}" +
                           @$"   {RenderOpcionesMenu(menu)}{Environment.NewLine}" +
                           @$"</ul>{Environment.NewLine}";
            return menuHtml.Replace("¨", "\"");
        }

        private static string RenderOpcionesMenu(List<FuncionalidadDto> opcionesMenu)
        {
            var menuHtml = "";
            foreach (FuncionalidadDto fDto in opcionesMenu)
            {
                menuHtml = menuHtml + RenderMenu(funcion: fDto);
            }
            return menuHtml;
        }

        private static string RenderMenu(FuncionalidadDto funcion)
        {

            var opcionHtml = "";
            if (funcion.Accion != null)
            {
                opcionHtml = RenderAccionMenu(accion: funcion.Accion);
                return opcionHtml;
            }
            
            var subMenuHtml = funcion.Opciones != null ? RenderOpcionesMenu(funcion.Opciones) : "";

            var idMenuHtml = $"id_menu_{funcion.Nombre.Replace(" ", "_")}".ToLower();
            var liHtml =
                $@"<li>{Environment.NewLine}" +
                $@"  <a>{Environment.NewLine}" +
                $@"     {ComponerMenu(literalOpcion: funcion.Nombre, icono: "cog-solid.svg", idMenu: idMenuHtml)}" + 
                $@"  </a>{Environment.NewLine}" +
                $@"  <ul id=¨{idMenuHtml}¨ name=¨menu¨ menu-plegado=¨true¨>{Environment.NewLine}" +
                      subMenuHtml +
                $@"  </ul>{Environment.NewLine}" +
                $@"</li>{Environment.NewLine}";

            return liHtml;
        }

        private static string RenderAccionMenu(AccionDto accion)
        {
            var idHtml = $"{accion.Controlador}_{accion.Accion}_{ accion.Id}".ToLower();
            var opcionHtml =
            $@"<li>{Environment.NewLine}"+
            $@"  <input id='{idHtml}' type='button' class='menu-opcion' value='{accion.Nombre}' onclick=¨Menu.OpcionSeleccionada('{idHtml}','{accion.Controlador}','{accion.Accion}')¨ />{Environment.NewLine}"+
            $@"</li>{Environment.NewLine}";

            return opcionHtml;
        }

        private static object ComponerMenu(string literalOpcion, string icono, string idMenu)
        {
            var opcionHtml = "";

            if (!icono.IsNullOrEmpty())
                opcionHtml = @$"<img src=¨/images/menu/{icono}¨ class=¨icono izquierdo¨ />{Environment.NewLine}";

            opcionHtml = $@"{opcionHtml}{literalOpcion}{Environment.NewLine}";

            if (!idMenu.IsNullOrEmpty())
                opcionHtml = $"{opcionHtml}<img src=¨/images/menu/angle-down-solid.svg¨ class=¨icono derecho¨ onclick=¨Menu.MenuPulsado('{idMenu}')¨/>{Environment.NewLine}";

            return opcionHtml;
        }

    }
}
