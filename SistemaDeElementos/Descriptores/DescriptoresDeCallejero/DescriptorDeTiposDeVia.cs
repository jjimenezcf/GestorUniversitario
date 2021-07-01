using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorTiposDeVia : DescriptorDeCrud<TipoDeViaDto>
    {
        public DescriptorTiposDeVia(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(TiposDeViaController)
               , nameof(TiposDeViaController.CrudTiposDeVia)
               , modo
               , rutaBase: "Callejero")
        {
            new EditorFiltro<TipoDeViaDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Sigla"
                , propiedad: nameof(TipoDeViaDto.Sigla)
                , ayuda: "buscar por sigla"
                , new Posicion { fila = 0, columna = 0 });
            RecolocarControl(Mnt.Filtro.FiltroDeNombre, new Posicion(0, 1), "T.Vía", "buscar por tipo de vía");            
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/TiposDeVia.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudTiposDeVia('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
