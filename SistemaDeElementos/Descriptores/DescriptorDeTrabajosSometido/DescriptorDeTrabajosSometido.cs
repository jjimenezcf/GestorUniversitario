using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrabajosSometido : DescriptorDeCrud<TrabajoSometidoDto>
    {
        public DescriptorDeTrabajosSometido(ModoDescriptor modo)
        : base(controlador: nameof(TrabajosSometidoController)
               , vista: $"{nameof(TrabajosSometidoController.CrudDeTrabajosSometido)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/TrabajosSometido.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeTrabajosSometido('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
