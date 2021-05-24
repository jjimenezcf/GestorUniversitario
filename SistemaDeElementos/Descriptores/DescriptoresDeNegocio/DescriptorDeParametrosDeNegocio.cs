using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Negocio;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeParametrosDeNegocio : DescriptorDeCrud<ParametroDeNegocioDto>
    {
        public DescriptorDeParametrosDeNegocio(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , controlador: nameof(ParametrosDeNegocioController)
               , vista: $"{nameof(ParametrosDeNegocioController.CrudDeParametrosDeNegocio)}"
               , modo: modo
              , rutaBase: "Negocio")
        {

            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            new RestrictorDeFiltro<ParametroDeNegocioDto>(fltGeneral, "Negocio", nameof(ParametroDeNegocioDto.IdNegocio), "parámetros del negocio", new Posicion(0, 0));
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/ParametrosDeNegocio.js¨></script>
                      <script>
                         try {{                           
                            Negocio.CrearCrudDeParametrosDeNegocio('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
