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
                , new Posicion { fila = 0, columna = 1 });

            Mnt.Datos.ExpresionElemento = $"([{nameof(VistaMvcDto.Nombre)}])";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Entorno/VistaMvc.js¨></script>
                      <script>
                         Crud.crudMnt = new Entorno.CrudMntVistaMvc('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}');
                         if (Crud.crudMnt === undefined)
                             alert('crud mal definido');
                      </script>
                    ";
            return render.Render();
        }
    }
}
