namespace MVCSistemaDeElementos.Descriptores
{
    public class CabeceraDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }

        public string IdHtml => $"cabecera-{Formulario.Id}".ToLower();

        public CabeceraDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderCabecera()
        {
            return $@"";
        }
    }
}