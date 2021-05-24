using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Negocio;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeNegocio : DescriptorDeCrud<NegocioDto>
    {
        public DescriptorDeNegocio(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , controlador: nameof(NegocioController)
               , vista: $"{nameof(NegocioController.CrudDeNegocios)}"
               , modo: modo
              , rutaBase: "Negocio")
        {
            AnadirOpcionDeDependencias(Mnt
            , controlador: nameof(ParametrosDeNegocioController)
            , vista: nameof(ParametrosDeNegocioController.CrudDeParametrosDeNegocio)
            , datosDependientes: nameof(ParametroDeNegocioDto)
            , navegarAlCrud: DescriptorDeMantenimiento<ParametroDeNegocioDto>.NombreMnt
            , nombreOpcion: "Parámetros"
            , propiedadQueRestringe: nameof(NegocioDto.Id)
            , propiedadRestrictora: nameof(ParametroDeNegocioDto.IdNegocio)
            , "Parametros de un negocio");
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Negocio.js¨></script>
                      <script>
                         try {{                           
                            Negocio.CrearCrudDeNegocios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
