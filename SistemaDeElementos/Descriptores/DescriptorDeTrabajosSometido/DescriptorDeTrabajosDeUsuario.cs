using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrabajosDeUsuario : DescriptorDeCrud<TrabajoDeUsuarioDto>
    {
        public DescriptorDeTrabajosDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(TrabajosDeUsuarioController)
               , vista: $"{nameof(TrabajosDeUsuarioController.CrudDeTrabajosDeUsuario)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/TrabajosDeUsuario.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeTrabajosDeUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
