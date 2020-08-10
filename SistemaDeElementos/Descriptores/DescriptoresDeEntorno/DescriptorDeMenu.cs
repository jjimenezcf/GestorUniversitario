using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeMenu : DescriptorDeCrud<MenuDto>
    {
        public DescriptorDeMenu(ModoDescriptor modo)
        : base(controlador: nameof(MenusController)
              , vista: nameof(MenusController.CrudMenu)
              , modo: modo)
        {

            var fltEspecificos = new BloqueDeFitro<MenuDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 4));

            new ListaDeElemento<MenuDto>(padre: fltEspecificos,
                                          propiedad: nameof(MenuDto.Padre),
                                          posicion: new Posicion() { fila = 0, columna = 0 });

            RutaVista = "Entorno";
        }
    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
                   $@"<script src=¨../../ts/Entorno/Menu.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudDeMenus('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
