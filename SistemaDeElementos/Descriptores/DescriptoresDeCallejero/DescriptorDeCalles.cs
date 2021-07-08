using ModeloDeDto.Callejero;
using ServicioDeDatos;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCalles : DescriptorDeCrud<CalleDto>
    {
        public DescriptorDeCalles(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(CallesController)
                 , nameof(CallesController.CrudCalles)
                 , modo
                 , rutaBase: "Callejero")
        {

            new ListasDinamicas<CalleDto>(Mnt.BloqueGeneral,
                etiqueta: "Pais",
                filtrarPor: nameof(CalleDto.IdPais),
                ayuda: "seleccione el país",
                seleccionarDe: nameof(PaisDto),
                buscarPor: nameof(PaisDto.Nombre),
                mostrarExpresion: $"([{nameof(PaisDto.Codigo)}]) [{nameof(PaisDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 0));

            new ListasDinamicas<CalleDto>(Mnt.BloqueGeneral,
                etiqueta: "Provincia",
                filtrarPor: nameof(CalleDto.IdProvincia),
                ayuda: "seleccione la provincia",
                seleccionarDe: nameof(ProvinciaDto),
                buscarPor: nameof(ProvinciaDto.Nombre),
                mostrarExpresion: $"([{nameof(ProvinciaDto.Codigo)}]) [{nameof(ProvinciaDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 1));

            new EditorFiltro<CalleDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(CalleDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 0, columna = 2 });

            //new EditorFiltro<CalleDto>(bloque: Mnt.BloqueGeneral
            //    , etiqueta: "CP"
            //    , propiedad: nameof(CpsDeUnCalleDto.CodigoPostal)
            //    , ayuda: "buscar por codigo postal"
            //    , new Posicion { fila = 1, columna = 1 });

            //AnadirOpciondeRelacion(Mnt
            //    , controlador: nameof(CpsDeUnMunicipioController)
            //    , vista: nameof(CpsDeUnMunicipioController.CrudCpsDeUnMunicipio)
            //    , relacionarCon: nameof(CodigoPostalDto)
            //    , navegarAlCrud: DescriptorDeMantenimiento<CpsDeUnCalleDto>.NombreMnt
            //    , nombreOpcion: "C.P."
            //    , propiedadQueRestringe: nameof(CalleDto.Id)
            //    , propiedadRestrictora: nameof(CpsDeUnCalleDto.IdMunicipio)
            //    , "Añadir puestos al usuario seleccionado");

            RecolocarControl(Mnt.Filtro.FiltroDeNombre, new Posicion(1, 0), "Calle", "Buscar por nombre de calle");
            Mnt.OrdenacionInicial = @$"{nameof(CalleDto.Pais)}:municipio.provincia.pais.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(CalleDto.Provincia)}:municipio.provincia.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(CalleDto.Provincia)}:municipio.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(CalleDto.Nombre)}:nombre:{enumModoOrdenacion.ascendente.Render()}";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/literales.js¨></script>
                      <script src=¨../../js/Callejero/Calles.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDeCalles('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
