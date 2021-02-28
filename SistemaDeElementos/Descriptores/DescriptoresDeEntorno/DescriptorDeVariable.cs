using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVariable : DescriptorDeCrud<VariableDto>
    {
        public DescriptorDeVariable(ModoDescriptor modo)
            :base(nameof(VariablesController),nameof(VariablesController.CrudVariable),modo, "Entorno")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new EditorFiltro<VariableDto>(bloque: fltGeneral
                , etiqueta: "Valor"
                , propiedad: nameof(VariableDto.Valor)
                , ayuda: "buscar por valor"
                , new Posicion { fila = 0, columna = 1 });
        }


    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
               $@"<script src=¨../../js/{RutaBase}/Variables.js¨></script>
                      <script>
                         try {{      
                           Entorno.CrearCrudDeVariables('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            Notificar(TipoMensaje.Error, error);
                         }}
                      </script>
                    ";
        return render.Render();
        }
    }

}
