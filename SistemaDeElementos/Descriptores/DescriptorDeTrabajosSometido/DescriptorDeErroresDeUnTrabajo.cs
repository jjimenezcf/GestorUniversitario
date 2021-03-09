using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeErroresDeUnTrabajo : DescriptorDeCrud<ErrorDeUnTrabajoDto>
    {
        public DescriptorDeErroresDeUnTrabajo(ModoDescriptor modo)
        : base(controlador: nameof(ErroresDeUnTrabajoController)
               , vista: $"{nameof(ErroresDeUnTrabajoController.CrudDeErroresDeUnTrabajo)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
            Mnt.ZonaMenu.QuitarOpcionDeMenu(TipoDeAccionDeMnt.CrearElemento);
            Mnt.ZonaMenu.QuitarOpcionDeMenu(TipoDeAccionDeMnt.EliminarElemento);

            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<ErrorDeUnTrabajoDto>(bloque: fltGeneral
                  , etiqueta: "Trabajo de Usuario"
                  , propiedad: nameof(ErrorDeUnTrabajoDto.IdTrabajoDeUsuario)
                  , ayuda: "buscar por trabajo de usuario"
                  , new Posicion { fila = 0, columna = 0 });

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/ErroresDeUnTrabajo.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeErroresDeUnTrabajo('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
