using ModeloDeDto;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using ServicioDeDatos.Callejero;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCpsDeUnMunicipio : DescriptorDeCrud<CpsDeUnMunicipioDto>
    {
        
        public DescriptorDeCpsDeUnMunicipio(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(CpsDeUnMunicipioController), nameof(CpsDeUnMunicipioController.CrudCpsDeUnMunicipio), modo, rutaBase: "Callejero")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            new RestrictorDeFiltro<CpsDeUnMunicipioDto>(bloque: fltGeneral
                  , etiqueta: "Municipio"
                  , propiedad:nameof(CpsDeUnMunicipioDto.IdMunicipio)
                  , ayuda: "buscar por municipio"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Código postal", nameof(CpsDeUnMunicipioDto.CodigoPostal), "Buscar por 'código postal'");

            var modalDePuestos = new ModalDeRelacionarElementos<CpsDeUnMunicipioDto, CodigoPostalDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los códigos postales a relacionar"
                              , crudModal: new DescriptorDeCodigosPostales(contexto,ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(CpsDeUnMunicipioDto.IdMunicipio));
            var relacionarCps = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl(), "Añadir códigos postales a la municipio");
            var opcion = new OpcionDeMenu<CpsDeUnMunicipioDto>(Mnt.ZonaMenu.Menu, relacionarCps, $"C.P.", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            Mnt.OrdenacionInicial = $"{nameof(CpsDeUnMunicipioDto.CodigoPostal)}:CodigoPostal.Codigo:{enumModoOrdenacion.ascendente.Render()}";

        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/CpsDeUnMunicipio.js¨></script>
                      <script>
                         try {{                           
                            {RutaBase}.CrearCrudDeCpsDeUnMunicipio('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
