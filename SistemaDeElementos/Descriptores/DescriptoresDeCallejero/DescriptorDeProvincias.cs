using ModeloDeDto.Callejero;
using SistemaDeElementos.Controllers.Callejero;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores.Callejero
{
    public class DescriptorDeProvincias : DescriptorDeCrud<ProvinciaDto>
    {
        public DescriptorDeProvincias(ModoDescriptor modo)
            : base(nameof(ProvinciasController)
                 , nameof(ProvinciasController.CrudProvincias)
                 , modo
                 , rutaBase: "Callejero")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");

            new ListasDinamicas<ProvinciaDto>(fltGeneral,
                etiqueta: "País",
                filtrarPor: nameof(ProvinciaDto.IdPais),
                ayuda: "seleccione al país",
                seleccionarDe: nameof(PaisDto),
                buscarPor: nameof(PaisDto.Nombre),
                mostrarExpresion: $"([{nameof(PaisDto.Codigo)}]) [{nameof(PaisDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 0));

            new EditorFiltro<ProvinciaDto>(bloque: fltGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(ProvinciaDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 1, columna = 0 });
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/Provincias.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDeProvincias('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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

