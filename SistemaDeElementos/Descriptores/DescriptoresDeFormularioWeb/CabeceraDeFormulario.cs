namespace MVCSistemaDeElementos.Descriptores
{
    public class CabeceraDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }

        public CabeceraDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderCabecera()
        {
            return $@"<h2>soy la cabecera del formulario {Formulario.Titulo}</h2>";
        }
    }
}