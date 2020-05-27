using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Elementos.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVariable : DescriptorDeCrud<VariableDto>
    {
        public DescriptorDeVariable(ModoDescriptor modo)
            :base(nameof(VariablesController),nameof(VariablesController.CrudVariable),modo)
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new EditorFiltro<VariableDto>(bloque: fltGeneral
                , etiqueta: "Valor"
                , propiedad: nameof(VariableDto.Valor)
                , ayuda: "buscar por valor"
                , new Posicion { fila = 0, columna = 1 });

           Mnt.Datos.ExpresionElemento = $"([{nameof(VariableDto.Nombre)}])";
        }


    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
               $@"<script src=¨../../ts/Entorno/Variables.js¨></script>
                      <script>
                         Crud.crudMnt = new Entorno.CrudMntVariable('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
        return render.Render();
        }
    }

}
