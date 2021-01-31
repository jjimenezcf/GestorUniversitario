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

            Cuerpo.Contenedores.Add(new ContenedorDeBloques(Cuerpo, "General", "Datos maestros"));
            var bloque = Cuerpo.Contenedores[0];
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvPais", "Fichero de paises", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar los paises"));
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvProvincia", "Fichero de provincias", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar provincias"));
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvMunicipio", "Fichero de municipio", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar municipios"));
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvTipoDeVias", "Fichero de tipos de vía", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar tipos de vías"));
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvCp", "Fichero de CP", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar los códigos postales"));

            bloque.Derecho.Add(new ControlDeFormulario(bloque, "csvPedanias", "Fichero de pedanias", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar los paises"));
            bloque.Derecho.Add(new ControlDeFormulario(bloque, "csvBarrios", "Fichero de barrios", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar los paises"));

            Cuerpo.Contenedores.Add(new ContenedorDeBloques(Cuerpo, "Otros", "Callejero"));
            bloque = Cuerpo.Contenedores[1];
            bloque.Izquierdo.Add(new ControlDeFormulario(bloque, "csvCalles", "Fichero de calles", enumCssControlesFormulario.Editor, "Selecciona un fichero para importar un callejero"));
        }

        public string RenderImportarCallejero()
        {
            var render = RenderFormulario();

            render = render +
                   $@"<script src=¨../../ts/Callejero/ImportarCallejero.js¨></script>
                      <script>
                         try {{                           
                            {RutaVista}.CrearFormulario('{IdHtml}') 
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
