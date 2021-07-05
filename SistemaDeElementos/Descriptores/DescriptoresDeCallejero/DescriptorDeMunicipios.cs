using ModeloDeDto.Callejero;
using ServicioDeDatos;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
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
                etiqueta: "Pais",
                filtrarPor: nameof(MunicipioDto.IdPais),
                ayuda: "seleccione el país",
                seleccionarDe: nameof(PaisDto),
                buscarPor: nameof(PaisDto.Nombre),
                mostrarExpresion: $"([{nameof(PaisDto.Codigo)}]) [{nameof(PaisDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 0));

            new ListasDinamicas<MunicipioDto>(Mnt.BloqueGeneral,
                etiqueta: "Provincia",
                filtrarPor: nameof(MunicipioDto.IdProvincia),
                ayuda: "seleccione la provincia",
                seleccionarDe: nameof(ProvinciaDto),
                buscarPor: nameof(ProvinciaDto.Nombre),
                mostrarExpresion: $"([{nameof(ProvinciaDto.Codigo)}]) [{nameof(ProvinciaDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 1));

            new EditorFiltro<MunicipioDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(MunicipioDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 0, columna = 2 });

            new EditorFiltro<MunicipioDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "CP"
                , propiedad: nameof(CpsDeUnMunicipioDto.CodigoPostal)
                , ayuda: "buscar por codigo postal"
                , new Posicion { fila = 1, columna = 1 });

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(CpsDeUnMunicipioController)
                , vista: nameof(CpsDeUnMunicipioController.CrudCpsDeUnMunicipio)
                , relacionarCon: nameof(CodigoPostalDto)
                , navegarAlCrud: DescriptorDeMantenimiento<CpsDeUnMunicipioDto>.NombreMnt
                , nombreOpcion: "C.P."
                , propiedadQueRestringe: nameof(MunicipioDto.Id)
                , propiedadRestrictora: nameof(CpsDeUnMunicipioDto.IdMunicipio)
                , "Añadir puestos al usuario seleccionado");

            RecolocarControl(Mnt.Filtro.FiltroDeNombre, new Posicion(1, 0), "Municipio", "Buscar por nombre de municipio");
            Mnt.OrdenacionInicial = @$"{nameof(MunicipioDto.Pais)}:provincia.pais.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(MunicipioDto.Provincia)}:provincia.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(MunicipioDto.Nombre)}:nombre:{enumModoOrdenacion.ascendente.Render()}";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/literales.js¨></script>
                      <script src=¨../../js/Callejero/Municipios.js¨></script>
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
