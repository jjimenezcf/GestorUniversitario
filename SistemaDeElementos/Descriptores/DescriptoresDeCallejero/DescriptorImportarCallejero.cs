using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorImportarCallejero: DescriptorDeFormulario
    {
        public DescriptorImportarCallejero():
            base(idHtml: "importar-callejero",
                titulo: "Importar Callejero",
                controlador: nameof(ImportarCallejeroController),
                ruta: "Callejero",
                vista: nameof(ImportarCallejeroController.ImportarCallejero))
        {

        }

        public string RenderImportarCallejero()
        {
            var render = RenderFormulario();

            render = render +
                   $@"<script src=¨../../ts/Callejero/ImportarCallejero.js¨></script>
                      <script>
                         try {{                           
                            {RutaVista}.CrearFormulario('{Id}') 
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
