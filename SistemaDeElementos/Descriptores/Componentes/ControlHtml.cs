namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoControl
    {
        Selector,
        Editor,
        Desplegable,
        GridModal,
        TablaBloque,
        Bloque,
        ZonaDeOpciones,
        ZonaDeGrid,
        ZonaDeFiltro,
        VistaCrud,
        DescriptorDeCrud,
        Opcion,
        Label,
        Referencia,
        Lista,
        Fecha,
        Plantilla
    }

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
        public TipoControl Tipo { get; protected set; }

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
            Id = $"{Padre.Id}_{propiedad}";
            Propiedad = propiedad;
            Ayuda = ayuda;
        }

    }

}