using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestosDeUnUsuario : DescriptorDeCrud<PuestosDeUnUsuarioDto>
    {
        
        public DescriptorDePuestosDeUnUsuario(ModoDescriptor modo)
        : base(nameof(PuestosDeUnUsuarioController), nameof(PuestosDeUnUsuarioController.CrudPuestosDeUnUsuario), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestosDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad:nameof(PuestosDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(RolesDeUnPuestoController)
                , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
                , relacionarCon: nameof(RolDto)
                , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.nombreMnt
                , nombreOpcion: "Roles"
                , propiedadQueRestringe: nameof(PuestosDeUnUsuarioDto.IdPuesto)
                , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestosDeUnUsuario.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
