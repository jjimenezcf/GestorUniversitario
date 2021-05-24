using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Negocio;
using ModeloDeDto.Entorno;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeAuditoria : DescriptorDeCrud<AuditoriaDto>
    {
        public DescriptorDeAuditoria(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , controlador: nameof(AuditoriaController)
               , vista: $"{nameof(AuditoriaController.CrudDeAuditoria)}"
               , modo: modo
              , rutaBase: "Negocio")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            new RestrictorDeFiltro<AuditoriaDto>(bloque: fltGeneral
                  , etiqueta: "Negocio"
                  , propiedad: NegocioPor.idNegocio
                  , ayuda: "negocio del elemento"
                  , new Posicion { fila = 0, columna = 0 });
            new RestrictorDeFiltro<AuditoriaDto>(bloque: fltGeneral
                  , etiqueta: "Elemento"
                  , propiedad: nameof(AuditoriaDto.IdElemento)
                  , ayuda: "elemento auditado"
                  , new Posicion { fila = 0, columna = 1 });

            var modalUsuario = new DescriptorDeUsuario(Contexto, ModoDescriptor.SeleccionarParaFiltrar);
            new SelectorDeFiltro<AuditoriaDto, UsuarioDto>(padre: fltGeneral,
                                              etiqueta: "Usuario",
                                              filtrarPor: UsuariosPor.AlgunUsuario,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 1, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.NombreCompleto),
                                              crudModal: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
        }
        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Auditoria.js¨></script>
                      <script>
                         try {{                           
                            Auditoria.CrearCrudDeAuditoria('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}','{Borrado.IdHtml}') 
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
