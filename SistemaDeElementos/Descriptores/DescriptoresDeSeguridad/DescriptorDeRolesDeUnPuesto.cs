using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRolesDeUnPuesto : DescriptorDeCrud<RolesDeUnPuestoDto>
    {
        public DescriptorDeRolesDeUnPuesto(ModoDescriptor modo)
        : base(nameof(RolesDeUnPuestoController), nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<RolesDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad:nameof(RolesDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por puesto"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Rol", nameof(RolesDeUnPuestoDto.Rol), "Buscar por 'rol'");

            var modalDeRoles = new ModalDeRelacionarElementos<RolesDeUnPuestoDto, RolDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los roles a relacionar"
                              , crudModal: new DescriptorDeRol(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));

            var relacionarRoles = new RelacionarElementos(modalDeRoles.IdHtml, () => modalDeRoles.RenderControl());
            var opcion = new OpcionDeMenu<RolesDeUnPuestoDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Roles", enumModoDeAccesoDeDatos.Gestor, enumCssOpcionMenu.DeVista);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PermisosDeUnRolController)
                , vista: nameof(PermisosDeUnRolController.CrudPermisosDeUnRol)
                , relacionarCon: nameof(PermisoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PermisosDeUnRolDto>.NombreMnt
                , nombreOpcion: "Permisos"
                , propiedadQueRestringe: nameof(RolesDeUnPuestoDto.IdRol )
                , propiedadRestrictora: nameof(PermisosDeUnRolDto.IdRol));

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/RolesDeUnPuesto.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeRolesDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
