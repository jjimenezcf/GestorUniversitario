using ModeloDeDto;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers.Callejero;
using ServicioDeDatos;
using SistemaDeElementos.Controllers.Callejero;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores.Callejero
{
    public class DescriptorDePais : DescriptorDeCrud<PaisDto>
    {
        public DescriptorDePais(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(PaisesController)
               , nameof(PaisesController.CrudPaises)
               , modo
               , rutaBase: "Callejero")
        {            
            new EditorFiltro<PaisDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(PaisDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 1, columna = 0 });

            AnadirOpcionDeDependencias(Mnt
                , controlador: nameof(ProvinciasController)
                , vista: nameof(ProvinciasController.CrudProvincias)
                , datosDependientes: nameof(ProvinciaDto)
                , navegarAlCrud: DescriptorDeMantenimiento<ProvinciaDto>.NombreMnt
                , nombreOpcion: "Provincias"
                , propiedadQueRestringe: nameof(PaisDto.Id)
                , propiedadRestrictora: nameof(ProvinciaDto.IdPais)
                , "Provincias de un pais");
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/Paises.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDePaises('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
