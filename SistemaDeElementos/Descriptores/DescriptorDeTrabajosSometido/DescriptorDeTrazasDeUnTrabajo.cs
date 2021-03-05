using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrazasDeUnTrabajo : DescriptorDeCrud<TrazaDeUnTrabajoDto>
    {
        public DescriptorDeTrazasDeUnTrabajo(ModoDescriptor modo)
        : base(controlador: nameof(TrazasDeUnTrabajoController)
               , vista: $"{nameof(TrazasDeUnTrabajoController.CrudDeTrazasDeUnTrabajo)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/TrazasDeUnTrabajo.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeTrazasDeUnTrabajo('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
