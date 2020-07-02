using Gestor.Elementos.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRol : DescriptorDeCrud<RolDto>
    {
        public DescriptorDeRol(ModoDescriptor modo)
            : base(nameof(RolController), nameof(RolController.CrudRol), modo)
        {
            RutaVista = "Seguridad";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/Rol.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntRol('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }
    }


}
