using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Negocio;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeNegocio : DescriptorDeCrud<NegocioDto>
    {
        public DescriptorDeNegocio(ModoDescriptor modo)
        : base(controlador: nameof(NegocioController)
               , vista: $"{nameof(NegocioController.CrudDeNegocios)}"
               , modo: modo)
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Negocio/Negocio.js¨></script>
                      <script>
                         try {{                           
                            Negocio.CrearCrudDeNegocios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            Mensaje(TipoMensaje.Error, error);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
