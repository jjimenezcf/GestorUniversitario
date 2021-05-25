using System;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum enumAccionDeFormulario { Cerrar, Aceptar }

    static class AccionDeFormularioExtension
    {
        public static string Render(this enumAccionDeFormulario accion)
        {
            switch (accion)
            {
                case enumAccionDeFormulario.Cerrar: return $"javascript:Formulario.{GestorDeEventos.EventosDelFormulario}('{TipoDeAccionFormulario.Cerrar}');";
                case enumAccionDeFormulario.Aceptar: return $"javascript:Formulario.{GestorDeEventos.EventosDelFormulario}('{TipoDeAccionFormulario.Aceptar}');";
            }

            throw new Exception($"No se ha definido como renderizar el tipo de input {accion}");
        }
    }

    public class OpcionDeFormulario
    {
        MenuDeFormulario Menu { get; }

        public string Id { get; }
        public string Etiqueta { get; }
        public enumAccionDeFormulario Accion { get; }

        public string IdHtml => $"{Menu.IdHtml}-{Id}".ToLower();

        public object Ayuda { get; }

        public OpcionDeFormulario(MenuDeFormulario menu, string id, string etiqueta, enumAccionDeFormulario accion, string ayuda)
        {
            Menu = menu;
            Id = id;
            Etiqueta = etiqueta;
            Accion = accion;
            Ayuda = ayuda;

            new OpcionHtml(menu, id, etiqueta, ayuda, accion.Render());

        }

        public string RenderOpcion()
        {

            

            return $@"<div id = ¨{IdHtml}¨ class=¨{Css.Render(enumCssControlesFormulario.ContenedorOpcion)}¨>
                        <input id=¨{IdHtml}¨ 
                               type=¨button¨ 
                               class=¨{Css.Render(enumCssOpcionMenu.Basico)}¨ 
                               value=¨{Etiqueta}¨ 
                               onClick=¨{Accion.Render()}¨
                               title=¨{Ayuda}¨/>
                      </div>
                     ";
        }
    }
}