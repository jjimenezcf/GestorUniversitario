namespace MVCSistemaDeElementos.Descriptores
{
    public class CuerpoDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }

        public CuerpoDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderCuerpo()
        {
            return $@"<h2>soy el cuerpo del formulario {Formulario.Titulo}</h2>";
        }
    }
}