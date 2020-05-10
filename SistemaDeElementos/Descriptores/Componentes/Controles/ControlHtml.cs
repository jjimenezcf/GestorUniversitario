namespace MVCSistemaDeElementos.Descriptores
{
    public static class TipoControl
    {
        public const string Selector = "selector";
        public const string SelectorDeElemento = "selector-de-elemento";
        public const string Editor = "editor";
        public const string Desplegable = "desplegable";
        public const string GridModal = "grid-modal";
        public const string TablaBloque = "tabla-bloque";
        public const string Bloque = "bloque";
        public const string ZonaDeOpciones = "zona-de-opciones";
        public const string ZonaDeDatos = "zona-de-datos";
        public const string ZonaDeFiltro = "zona-de-filtro";
        public const string VistaCrud = "vista-crud";
        public const string DescriptorDeCrud = "descriptor-crud";
        public const string Opcion = "opcion";
        public const string Label = "label";
        public const string Referencia = "referencia";
        public const string Lista = "lista";
        public const string Fecha = "fecha";
        public const string Plantilla = "plantilla";
        public const string Mantenimiento = "mantenimiento";
        public const string pnlCreador = "panel-creador";
        public const string pnlEditor = "panel-editor";
        public const string pnlBorrado = "panel-borrado";
        public const string ZonaMenu = "zona-menu";
        public const string Menu = "menu";    }

    public class Posicion
    {
        public int fila { get; set; }
        public int columna { get; set; }
    }

    public abstract class ControlHtml
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; private set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; private set; }
        public Posicion Posicion { get; private set; }
        public string Tipo { get; protected set; }

        public string TipoHtml => Tipo.ToString().ToLower();

        public ControlHtml Padre { get; set; }

        public ControlHtml(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Padre = padre;
            Id = id;
            Etiqueta = etiqueta;
            Propiedad = propiedad;
            Ayuda = ayuda;
            Posicion = posicion;
        }

        public string RenderLabel()
        {
            return $@"<div class=¨input-group mb-3¨>
                         {Etiqueta}
                      </div>
                  ";
        }

        public abstract string RenderControl();

        public virtual string RenderAtributos(string atributos = "")
        {
            atributos += $"tipo=¨{TipoHtml}¨ propiedad=¨{Propiedad}¨ ";
            return atributos;
        }

        public void CambiarAtributos(string propiedad, string ayuda)
        {
            Id = $"{Padre.Id}_{Tipo}_{propiedad}";
            Propiedad = propiedad;
            Ayuda = ayuda;
        }

    }

}