using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePermisosDeUnPuesto : DescriptorDeCrud<PermisosDeUnPuestoDto>
    {
        public DescriptorDePermisosDeUnPuesto(ModoDescriptor modo)
        : base(nameof(PermisosDeUnPuestoController), nameof(PermisosDeUnPuestoController.CrudPermisosDeUnPuesto), modo)
        {
            RutaVista = "Entorno";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PermisosDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad: nameof(PermisosDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por Puesto"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos("Permiso", nameof(PermisosDeUnPuestoDto.Permiso), "Buscar por 'permiso'");

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PermisosDeUnPuesto.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePermisosDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            Mensaje(TipoMensaje.Error, error);
                         }}
                      </script>
                    ";
            return render.Render();
        }
    }
}
