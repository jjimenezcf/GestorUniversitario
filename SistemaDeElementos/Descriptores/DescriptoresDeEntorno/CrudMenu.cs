using Gestor.Elementos.Entorno;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudMenu : DescriptorDeCrud<MenuDto>
    {
        public CrudMenu(ModoDescriptor modo)
        : base(controlador: nameof(MenusController)
              , vista: nameof(MenusController.CrudMenu)
              , modo: modo)
        {

            Mnt.Datos.ExpresionElemento = $"[{nameof(MenuDto.Nombre)}]";
        }
    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
                   $@"<script src=¨../../ts/Entorno/Menu.js¨></script>
                      <script>
                         Crud.crudMnt = new Entorno.CrudMntMenu('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                     ";
        return render.Render();
        }

    }
}
