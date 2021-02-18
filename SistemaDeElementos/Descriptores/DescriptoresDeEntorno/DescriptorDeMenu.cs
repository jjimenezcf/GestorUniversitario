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
              , modo: modo
               , rutaBase: "Entorno")
        {

            var fltEspecificos = new BloqueDeFitro<MenuDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2));

            new ListasDinamicas<MenuDto>(bloque: fltEspecificos,
                etiqueta:"Menu padre",
                filtrarPor: nameof(MenuDto.idPadre),
                ayuda:"seleccionar padre",
                seleccionarDe: nameof(MenuDto),
                buscarPor: nameof(MenuDto.Nombre),
                mostrarExpresion: $"[{nameof(MenuDto.Padre)}].[{nameof(MenuDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion() { fila = 0, columna = 0 });

            new CheckFiltro<MenuDto>(bloque: fltEspecificos,
                etiqueta: "Mostrar las activas",
                filtrarPor: nameof(MenuDto.Activo),
                ayuda: "Sólo las activos",
                valorInicial: false,
                filtrarPorFalse: false,
                posicion: new Posicion(0, 1));
        }
    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
                  $@"<script src=¨../../js/{RutaBase}/Menu.js¨></script>
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

/*
 * 
                    
*/
