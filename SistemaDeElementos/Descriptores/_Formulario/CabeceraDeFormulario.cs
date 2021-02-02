namespace MVCSistemaDeElementos.Descriptores
{
    public class CabeceraDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }
        public MenuDeFormulario Menu { get; private set; }

        public string IdHtml => $"cabecera-{Formulario.Id}".ToLower();

        public CabeceraDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
            Menu = new MenuDeFormulario(this);
        }

        public string RenderCabecera()
        {
            return $@"{Menu.RenderMenu()}";
        }
    }
}