using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVistaMvc : DescriptorDeCrud<VistaMvcDto>
    {
        public DescriptorDeVistaMvc(ModoDescriptor modo)
            : base(nameof(VistaMvcController), nameof(VistaMvcController.CrudVistaMvc), modo)
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new EditorFiltro<VistaMvcDto>(bloque: fltGeneral
                , etiqueta: "Controlador"
                , propiedad: nameof(VistaMvcDto.Controlador)
                , ayuda: "buscar por controlador"
                , new Posicion { fila = 1, columna = 0 });

            RutaVista = "Entorno";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Entorno/VistaMvc.js¨></script>
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
