using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeErroresDeUnTrabajo : DescriptorDeCrud<ErrorDeUnTrabajoDto>
    {
        public DescriptorDeErroresDeUnTrabajo(ModoDescriptor modo)
        : base(controlador: nameof(ErroresDeUnTrabajoController)
               , vista: $"{nameof(ErroresDeUnTrabajoController.CrudDeErroresDeUnTrabajo)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/ErroresDeUnTrabajo.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeErroresDeUnTrabajo('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
