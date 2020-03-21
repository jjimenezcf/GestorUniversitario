﻿using System;
using System.Collections.Generic;
using System.IO;
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
                           @$"   {RenderOpcionesMenu(menu, 0)}{Environment.NewLine}" +
                           @$"</ul>{Environment.NewLine}";
            return menuHtml.Replace("¨", "\"");
        }

        private static string RenderOpcionesMenu(List<Funcion> opcionesMenu, int idMenuPadre)
        {
            var menuHtml = "";
            foreach (Funcion fDto in opcionesMenu)
            {
                menuHtml = menuHtml + RenderMenu(funcion: fDto, idMenuPadre);
            }
            return menuHtml;
        }

        private static string RenderMenu(Funcion funcion, int idMenuPadre)
        {
            if (funcion.Accion != null)
            {
                var opcionHtml = RenderAccionMenu(accion: funcion.Accion);
                return opcionHtml;
            }
            
            var subMenuHtml = funcion.Opciones != null ? RenderOpcionesMenu(funcion.Opciones, funcion.Id) : "";

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

        private static string RenderAccionMenu(EleAccion accion)
        {
            var idHtml = $"{accion.Id}";
            var opcionHtml =
            $@"<li>{Environment.NewLine}"+
            $@"  <input id='{idHtml}' type='button' class='menu-opcion' value='{accion.Nombre}' onclick=¨Menu.OpcionSeleccionada('{idHtml}','{accion.Controlador}','{accion.Accion}')¨ />{Environment.NewLine}"+
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
