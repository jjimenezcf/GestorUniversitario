using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVistaMvc : DescriptorDeCrud<VistaMvcDto>
    {
        public DescriptorDeVistaMvc(ModoDescriptor modo)
            : base(nameof(VistaMvcController), nameof(VistaMvcController.CrudVistaMvc), modo, "Entorno")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            
            new EditorFiltro<VistaMvcDto>(bloque: fltGeneral
                , etiqueta: "Controlador"
                , propiedad: nameof(VistaMvcDto.Controlador)
                , ayuda: "buscar por controlador"
                , new Posicion { fila = 1, columna = 0 });

            new CheckFiltro<VistaMvcDto>(bloque: fltGeneral,
                etiqueta: "Mostrar solo las modales",
                filtrarPor: nameof(VistaMvcDto.MostrarEnModal),
                ayuda: "Sólo las las modales",
                valorInicial: false,
                filtrarPorFalse: false,
                posicion: new Posicion(1, 1));
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/VistaMvc.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudVistaMvc('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}');
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
