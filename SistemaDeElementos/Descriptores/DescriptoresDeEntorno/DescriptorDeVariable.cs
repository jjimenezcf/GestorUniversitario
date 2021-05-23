using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVariable : DescriptorDeCrud<VariableDto>
    {
        public DescriptorDeVariable(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(VariablesController),nameof(VariablesController.CrudVariable),modo, "Entorno")
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
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
        return render.Render();
        }
    }

}
