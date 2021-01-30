namespace MVCSistemaDeElementos.Descriptores
{
    public class PieDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }

        public PieDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderPie()
        {
            return $@"<h2>soy el pie del formulario {Formulario.Titulo}</h2>";
        }
    }
}