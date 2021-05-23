using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeClaseDePermiso : DescriptorDeCrud<ClasePermisoDto>
    {
        public DescriptorDeClaseDePermiso(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(ClaseDePermisoController), nameof(ClaseDePermisoController.CrudClaseDePermiso), modo, "Seguridad")
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
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
            return render.Render();
        }

    }
}
