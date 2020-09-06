using System;
using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestoresDeNegocio.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class MenusController : EntidadController<ContextoSe, MenuDtm, MenuDto>
    {
        public GestorDeMenus GestorDeMenus { get; set; }

        public MenusController(GestorDeMenus gestorDeMenus, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeMenus,
          gestorDeErrores,
          new DescriptorDeMenu(ModoDescriptor.Mantenimiento)
        )
        {
            GestorDeMenus = gestorDeMenus;
        }

        public IActionResult CrudMenu()
        {
            return ViewCrud();
        }


        protected override dynamic CargarLista(string claseElemento)
        {
            if (claseElemento == nameof(MenuDto))
                return ((GestorDeMenus)GestorDeElementos).LeerPadres();

            return null;
        }


        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(VistaMvcDto))
                return ((GestorDeMenus)GestorDeElementos).LeerVistas(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);

        }

        //END-POINT: Desde Menu.ts
        public JsonResult epSolicitarMenuHtml(string usuario)
        {
            var r = new ResultadoHtml();
            try
            {
                var procesadas = new List<int>();
                List<ArbolDeMenuDto> menu = GestorDeMenus.LeerArbolDeMenu(10, GestorDeElementos.Mapeador);
                var menuHtml = 
@$"
<ul id='id_menuraiz' class=¨menu-contenido¨>
   {RenderOpcionesMenu(menu, procesadas, 0)}
</ul>
";
                r.Html = menuHtml.Replace("¨", "\"");
                r.Estado = EstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = EstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = "No se ha podido leer el menú";
            }
            return new JsonResult(r);
        }

        private static string RenderOpcionesMenu(List<ArbolDeMenuDto> opcionesMenu, List<int> procesadas, int idMenuPadre)
        {
            var menuHtml = "";
            foreach (ArbolDeMenuDto fDto in opcionesMenu)
            {
                if (procesadas.Contains(fDto.Id))
                    continue;

                menuHtml = menuHtml + RenderMenu(funcion: fDto, procesadas, idMenuPadre);
                procesadas.Add(fDto.Id);
            }
            return menuHtml;
        }

        private static string RenderMenu(ArbolDeMenuDto funcion, List<int> procesadas, int idMenuPadre)
        {
            if (funcion.IdVistaMvc != null)
            {
                var opcionHtml = RenderAccionMenu(funcion);
                return opcionHtml;
            }

            var subMenuHtml = funcion.Submenus != null ? RenderOpcionesMenu(funcion.Submenus, procesadas, funcion.Id) : "";

            var idMenuHtml = $"id_menu_{funcion.Id}";
            var idMenuPadreHtml = $"id_menu_{idMenuPadre}";
            var liHtml =
$@"
<li>
    <a style=¨display: flex; padding-top: 2px;padding-bottom: 2px;¨>
      <img src=¨/images/menu/{funcion.Icono}¨ style=¨margin-top: 6px;margin-right: 0px; width: 20px; height: 20px;¨ />
      <input id='{funcion.Id}' type='button' class='menu-opcion' value='{funcion.Nombre}' style=¨padding-left: 3px;¨ onclick =¨ArbolDeMenu.MenuPulsado('{idMenuHtml}')¨ />
    </a>
    <ul id=¨{idMenuHtml}¨ name=¨menu¨ menu-padre=¨{idMenuPadreHtml}¨ menu-plegado=¨true¨>
      subMenuHtml
    </ul>
</li>
";

            return liHtml.Replace("subMenuHtml", subMenuHtml);
        }

        private static string RenderAccionMenu(ArbolDeMenuDto funcion)
        {
            var idHtml = $"{funcion.VistaMvc.Id}";
            var opcionHtml =
            $@"<li>{Environment.NewLine}" +
            $@"  <a style=¨display: flex; padding-top: 2px;padding-bottom: 2px;¨>{Environment.NewLine}" +
            $@"    <img src =¨/images/menu/{funcion.Icono}¨ style=¨margin-top: 6px;margin-right: 0px; width: 20px; height: 20px;¨ />" +
            $@"    <input id='{idHtml}' type='button' class='menu-opcion' value='{funcion.Nombre}' style=¨padding-left: 3px;¨ onclick =¨ArbolDeMenu.OpcionSeleccionada('{idHtml}','{funcion.VistaMvc.Controlador}','{funcion.VistaMvc.Accion}')¨ />{Environment.NewLine}" +
            $@"  </a>" +
            $@"</li>{Environment.NewLine}";

            return opcionHtml;
        }

    }
}