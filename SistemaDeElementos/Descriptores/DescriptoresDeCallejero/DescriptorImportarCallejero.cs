﻿using GestoresDeNegocio.Callejero;
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
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, GestorDePaises.ParametroPais, "Fichero de paises", "Selecciona un fichero para importar los paises", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, GestorDeProvincias.ParametroProvincia, "Fichero de provincias", "Selecciona un fichero para importar provincias", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvMunicipio", "Fichero de municipio", "Selecciona un fichero para importar municipios", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvTipoDeVias", "Fichero de tipos de vía", "Selecciona un fichero para importar tipos de vías", "*.csv"));
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvCp", "Fichero de CP", "Selecciona un fichero para importar los códigos postales", "*.csv"));

            bloque.Derecho.Add(new ControlDeArchivo(bloque, "csvPedanias", "Fichero de pedanias", "Selecciona un fichero para importar los pedanias", "*.csv"));
            bloque.Derecho.Add(new ControlDeArchivo(bloque, "csvBarrios", "Fichero de barrios", "Selecciona un fichero para importar los barrios", "*.csv"));

            Cuerpo.Contenedores.Add(new ContenedorDeBloques(Cuerpo, "Otros", "Callejero"));
            bloque = Cuerpo.Contenedores[1];
            bloque.Izquierdo.Add(new ControlDeArchivo(bloque, "csvCalles", "Fichero de calles", "Selecciona un fichero para importar un callejero", "*.csv"));
        }

        public string RenderImportarCallejero()
        {
            var render = RenderFormulario();

            render = render +
                   $@"<script src=¨../../js/Callejero/ImportarCallejero.js¨></script>
                      <script>
                         try {{                           
                            {RutaVista}.CrearFormulario('{IdHtml}') 
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el formulario', error);
                         }}
                      </script>
                    ";

            return render.Render();
        }

    }
}
