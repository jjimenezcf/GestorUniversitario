using ModeloDeDto;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers.Callejero;
using ServicioDeDatos;
using SistemaDeElementos.Controllers.Callejero;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores.Callejero
{
    public class DescriptorDeCodigosPostales : DescriptorDeCrud<CodigoPostalDto>
    {
        public DescriptorDeCodigosPostales(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(CodigosPostalesController)
               , nameof(CodigosPostalesController.CrudCodigosPostales)
               , modo
               , rutaBase: "Callejero")
        {            
           var opcionProvincias = AnadirOpcionDeDependencias(Mnt
                , controlador: nameof(ProvinciasController)
                , vista: nameof(ProvinciasController.CrudProvincias)
                , datosDependientes: nameof(ProvinciaDto)
                , navegarAlCrud: DescriptorDeMantenimiento<ProvinciaDto>.NombreMnt
                , nombreOpcion: "Provincias"
                , propiedadQueRestringe: nameof(CodigoPostalDto.Codigo)
                , propiedadRestrictora: nameof(CpsDeUnaProvinciaDto.CodigoPostal)
                , "Provincias de un CP");

            opcionProvincias.SoloMapearEnElFiltro = true;


           var opcionMunicipios = AnadirOpcionDeDependencias(Mnt
                , controlador: nameof(MunicipiosController)
                , vista: nameof(MunicipiosController.CrudMunicipios)
                , datosDependientes: nameof(MunicipioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<MunicipioDto>.NombreMnt
                , nombreOpcion: "Municipios"
                , propiedadQueRestringe: nameof(CodigoPostalDto.Codigo)
                , propiedadRestrictora: nameof(CpsDeUnMunicipioDto.CodigoPostal)
                , "Municipios de un CP");

            opcionMunicipios.SoloMapearEnElFiltro = true;

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Código Postal", nameof(CodigoPostalDto.Codigo), "Buscar por 'código postal'");

        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/CodigosPostales.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDeCodigosPostales('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
