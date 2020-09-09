using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
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

            var modalDeRoles = new ModalDeRelacionarElementos<RolesDeUnPermisoDto, RolDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los roles a relacionar"
                              , crudModal: new DescriptorDeRol(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(RolesDeUnPermisoDto.IdPermiso));
            var relacionarRoles = new RelacionarElementos(modalDeRoles.IdHtml, () => modalDeRoles.RenderControl());
            var opcion = new OpcionDeMenu<RolesDeUnPermisoDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Roles");
            Mnt.ZonaMenu.Menu.Add(opcion);

            //AnadirOpciondeRelacion(Mnt
            //    , controlador: nameof(RolesDeUnPuestoController)
            //    , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
            //    , relacionarCon: nameof(RolDto)
            //    , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.NombreMnt
            //    , nombreOpcion: "Roles"
            //    , propiedadQueRestringe: nameof(PuestosDeUnUsuarioDto.IdPuesto)
            //    , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));
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
