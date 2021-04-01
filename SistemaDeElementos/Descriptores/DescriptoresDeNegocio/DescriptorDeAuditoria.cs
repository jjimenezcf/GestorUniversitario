using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Negocio;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeAuditoria : DescriptorDeCrud<AuditoriaDto>
    {
        public DescriptorDeAuditoria(ModoDescriptor modo)
        : base(controlador: nameof(AuditoriaController)
               , vista: $"{nameof(AuditoriaController.CrudDeAuditoria)}"
               , modo: modo
              , rutaBase: "Negocio")
        {
        }
        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Auditoria.js¨></script>
                      <script>
                         try {{                           
                            Auditoria.CrearCrudDeAuditoria('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}','{Borrado.IdHtml}') 
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
