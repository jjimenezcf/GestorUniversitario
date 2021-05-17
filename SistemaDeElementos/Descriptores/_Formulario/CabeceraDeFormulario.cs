namespace MVCSistemaDeElementos.Descriptores
{
    public class CabeceraDeFormulario: ControlHtml
    {
        //DescriptorDeFormulario Formulario { get; }
        public MenuDeFormulario Menu { get; private set; }

        //public string IdHtml => $"cabecera-{Formulario.Id}".ToLower();

        public CabeceraDeFormulario(DescriptorDeFormulario formulario)
            :base(formulario, $"cabecera-{formulario.Id}","","","",null)
        {
            //Formulario = formulario;
            Menu = new MenuDeFormulario(this);
        }

        public string RenderCabecera()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
            return $@"{Menu.RenderMenu()}";
        }
    }
}