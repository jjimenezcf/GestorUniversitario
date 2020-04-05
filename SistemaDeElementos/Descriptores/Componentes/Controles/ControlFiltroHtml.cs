namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoCriterio { igual, contiene }

    public class ControlFiltroHtml : ControlHtml
    {
        public string Criterio { get; set; }
        public ControlFiltroHtml(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre, id, etiqueta, propiedad, ayuda, posicion)
        {
        }

        public override string RenderControl()
        {
            return "";
        }

        public override string RenderAtributos(string atributos = "")
        {
            atributos = base.RenderAtributos(atributos);
            atributos += $"filtro=¨S¨ criterio=¨{Criterio}¨ ";
            return atributos;
        }
    }
}
