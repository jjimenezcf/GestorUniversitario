using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePais : DescriptorDeCrud<PaisDto>
    {
        public DescriptorDePais(ModoDescriptor modo)
            :base(nameof(PaisesController)
                 ,nameof(PaisesController.CrudPaises)
                 , modo
                 , rutaBase: "Callejero")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new EditorFiltro<PaisDto>(bloque: fltGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(PaisDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 1, columna = 0 });
        }


    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
               $@"<script src=¨../../js/Callejero/Paises.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDePaises('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
