using ServicioDeDatos;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudMenu : DescriptorDeCrud<MenuDto>
    {
        public CrudMenu(ModoDescriptor modo)
        : base(controlador: nameof(MenusController)
              , vista: nameof(MenusController.CrudMenu)
              , modo: modo)
        {

            var fltEspecificos = new BloqueDeFitro<MenuDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 4));

            new SelectorDeElemento<MenuDto>(padre: fltEspecificos,
                                          propiedad: nameof(MenuDto.Padre),
                                          posicion: new Posicion() { fila = 0, columna = 0 });



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
