using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public class MenuDeFormulario
    {
        public CabeceraDeFormulario Cabecera { get; }

        public string IdHtml => $"menu-{Cabecera.IdHtml}".ToLower();

        public List<OpcionDeFormulario> Opciones { get; }



        public MenuDeFormulario(CabeceraDeFormulario cabecera)
        {
            Cabecera = cabecera;
            Opciones = new List<OpcionDeFormulario>();
            var opcionCerrar = new OpcionDeFormulario(this, "cerrar", "Cerrar", enumAccionDeFormulario.Cerrar, "Cerra sin procesar");
            var opcionAceptar = new OpcionDeFormulario(this, "aceptar", "Aceptar", enumAccionDeFormulario.Aceptar, "Cerrar y procesar formulario");
            Opciones.Add(opcionCerrar);
            Opciones.Add(opcionAceptar);
        }

        public string RenderMenu()
        {
            var menu = "";
            var i = 0;
            foreach (var opcion in Opciones)
            {
                menu = $@"{menu}{opcion.RenderOpcion()}";
                i++;
            }
            return $@"<div id=¨{IdHtml}¨ class=¨{Css.Render(enumCssControlesFormulario.Menu)}¨>{menu}</div>";
        }
    }
}