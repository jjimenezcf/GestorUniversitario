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
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvPais", "Fichero de paises", "Selecciona un fichero para importar los paises", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvProvincia", "Fichero de provincias", "Selecciona un fichero para importar provincias", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeEdicion(bloque, "csvMunicipio", "Fichero de municipio", "Selecciona un fichero para importar municipios"));
            bloque.Izquierdo.Add(new ControlDeEdicion(bloque, "csvTipoDeVias", "Fichero de tipos de vía", "Selecciona un fichero para importar tipos de vías"));
            bloque.Izquierdo.Add(new ControlDeEdicion(bloque, "csvCp", "Fichero de CP", "Selecciona un fichero para importar los códigos postales"));

            bloque.Derecho.Add(new ControlDeEdicion(bloque, "csvPedanias", "Fichero de pedanias", "Selecciona un fichero para importar los paises"));
            bloque.Derecho.Add(new ControlDeEdicion(bloque, "csvBarrios", "Fichero de barrios", "Selecciona un fichero para importar los paises"));

            Cuerpo.Contenedores.Add(new ContenedorDeBloques(Cuerpo, "Otros", "Callejero"));
            bloque = Cuerpo.Contenedores[1];
            bloque.Izquierdo.Add(new ControlDeEdicion(bloque, "csvCalles", "Fichero de calles", "Selecciona un fichero para importar un callejero"));
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
