using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePais : DescriptorDeCrud<PaisDto>
    {
        public DescriptorDePais(ModoDescriptor modo)
            :base(nameof(PaisesController),nameof(PaisesController.CrudPaises),modo)
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new EditorFiltro<PaisDto>(bloque: fltGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(PaisDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 0, columna = 1 });
            RutaVista = "Callejero";
        }


    public override string RenderControl()
    {
        var render = base.RenderControl();

        render = render +
               $@"<script src=¨../../ts/Callejero/Paises.js¨></script>
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
