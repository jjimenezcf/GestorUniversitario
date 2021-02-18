using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeClaseDePermiso : DescriptorDeCrud<ClasePermisoDto>
    {
        public DescriptorDeClaseDePermiso(ModoDescriptor modo)
            : base(nameof(ClaseDePermisoController), nameof(ClaseDePermisoController.CrudClaseDePermiso), modo, "Seguridad")
        {
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/ClaseDePermiso.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeClasesDePermiso('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
