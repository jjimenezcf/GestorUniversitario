namespace MVCSistemaDeElementos.Descriptores
{
    public class PieDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }
        public string IdHtml => $"pie-{Formulario.Id}".ToLower();

        public PieDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderPie()
        {
            return "";
        }
    }
}