using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCorreos : DescriptorDeCrud<CorreoDto>
    {
        public DescriptorDeCorreos(ModoDescriptor modo)
        : base(controlador: nameof(CorreosController)
               , vista: $"{nameof(CorreosController.CrudDeCorreos)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Correos.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeCorreos('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
