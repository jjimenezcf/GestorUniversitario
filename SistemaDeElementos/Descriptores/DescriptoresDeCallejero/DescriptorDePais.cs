using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers.Callejero;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores.Callejero
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

            var expanDeAuditoria = new DescriptorDeExpansor(Editor, $"{Editor.Id}-audt", "Auditoría", "Información de auditoría");
            Editor.Expanes.Add(expanDeAuditoria);

            var fechaCreacion = new EditorDeFecha(expanDeAuditoria, $"{expanDeAuditoria.Id}-feccre", "Creado el", "fecCre", "fecha de cuando se creó el elemento");
            expanDeAuditoria.Controles.Add(fechaCreacion);

            var fechaModificacion = new EditorDeFecha(expanDeAuditoria, $"{expanDeAuditoria.Id}-fecmod", "Modificado el", "fecMod", "fecha de cuando se modificó por última vez");
            expanDeAuditoria.Controles.Add(fechaModificacion);

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
                            MensajesSe.Error('Creando el crud', error);
                         }}
                      </script>
                    ";
        return render.Render();
        }
    }

}
