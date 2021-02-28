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
               , modo: modo
              , rutaBase: "Negocio")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Negocio.js¨></script>
                      <script>
                         try {{                           
                            Negocio.CrearCrudDeNegocios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            Notificar(TipoMensaje.Error, error);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
