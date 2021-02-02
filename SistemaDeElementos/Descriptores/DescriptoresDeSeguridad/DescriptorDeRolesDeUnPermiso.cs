using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRolesDeUnPermiso : DescriptorDeCrud<RolesDeUnPermisoDto>
    {

        public DescriptorDeRolesDeUnPermiso(ModoDescriptor modo)
        : base(nameof(RolesDeUnPermisoController), nameof(RolesDeUnPermisoController.CrudRolesDeUnPermiso), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<RolesDeUnPermisoDto>(bloque: fltGeneral
                  , etiqueta: "Permiso"
                  , propiedad: nameof(RolesDeUnPermisoDto.IdPermiso)
                  , ayuda: "buscar por permiso"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Rol", nameof(RolesDeUnPermisoDto.Rol), "Buscar por 'rol'");

            var modalDeRoles = new ModalDeRelacionarElementos<RolesDeUnPermisoDto, RolDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los roles a relacionar"
                              , crudModal: new DescriptorDeRol(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(RolesDeUnPermisoDto.IdPermiso));
            var relacionarRoles = new RelacionarElementos(modalDeRoles.IdHtml, () => modalDeRoles.RenderControl());
            var opcion = new OpcionDeMenu<RolesDeUnPermisoDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Roles", enumModoDeAccesoDeDatos.Gestor,enumCssOpcionMenu.DeVista, "Seleccionar los roles donde incluir el permiso");
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PuestosDeUnRolController)
                , vista: nameof(PuestosDeUnRolController.CrudPuestosDeUnRol)
                , relacionarCon: nameof(PuestoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PuestosDeUnRolDto>.NombreMnt
                , nombreOpcion: "Puestos"
                , propiedadQueRestringe: nameof(PuestosDeUnRolDto.IdRol)
                , propiedadRestrictora: nameof(PuestosDeUnRolDto.IdRol)
                , ayuda: "Incluir el rol a los puestos seleccionados");
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/RolesDeUnPermiso.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeRolesDeUnPermiso('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
