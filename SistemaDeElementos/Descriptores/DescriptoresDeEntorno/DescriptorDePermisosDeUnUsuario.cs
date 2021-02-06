using ModeloDeDto;
using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePermisosDeUnUsuario : DescriptorDeCrud<PermisosDeUnUsuarioDto>
    {
        public DescriptorDePermisosDeUnUsuario(ModoDescriptor modo)
        : base(nameof(PermisosDeUnUsuarioController), nameof(PermisosDeUnUsuarioController.CrudPermisosDeUnUsuario), modo, "Entorno")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PermisosDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad: nameof(PermisosDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Permiso", nameof(PermisosDeUnUsuarioDto.Permiso), "Buscar por 'permiso'");

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/{RutaBase}/PermisosDeUnUsuario.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudDePermisosDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
