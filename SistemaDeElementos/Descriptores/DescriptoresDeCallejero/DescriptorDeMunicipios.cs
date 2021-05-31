using ModeloDeDto.Callejero;
using ServicioDeDatos;
using SistemaDeElementos.Controllers.Callejero;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores.Callejero
{
    public class DescriptorDeMunicipios : DescriptorDeCrud<MunicipioDto>
    {
        public DescriptorDeMunicipios(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(MunicipiosController)
                 , nameof(MunicipiosController.CrudMunicipios)
                 , modo
                 , rutaBase: "Callejero")
        {


            new ListasDinamicas<MunicipioDto>(Mnt.BloqueGeneral,
                etiqueta: "Provincia",
                filtrarPor: nameof(MunicipioDto.IdProvincia),
                ayuda: "seleccione la provincia",
                seleccionarDe: nameof(ProvinciaDto),
                buscarPor: nameof(ProvinciaDto.Nombre),
                mostrarExpresion: $"([{nameof(ProvinciaDto.Codigo)}]) [{nameof(ProvinciaDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 0));

            new EditorFiltro<MunicipioDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(MunicipioDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 1, columna = 0 });
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/Municipios.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDeMunicipios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
