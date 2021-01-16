using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestoDeTrabajo : DescriptorDeCrud<PuestoDto>
    {
        public DescriptorDePuestoDeTrabajo(ModoDescriptor modo)
            : base(nameof(PuestoDeTrabajoController), nameof(PuestoDeTrabajoController.CrudPuestoDeTrabajo), modo)
        {
            RutaVista = "Seguridad";

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(UsuariosDeUnPuestoController)
                , vista: nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto)
                , relacionarCon: nameof(UsuarioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<UsuariosDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Usuarios"
                , propiedadQueRestringe: nameof(PuestoDto.Id)
                , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto));

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(RolesDeUnPuestoController)
                , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
                , relacionarCon: nameof(RolDto)
                , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Roles"
                , propiedadQueRestringe: nameof(PuestoDto.Id)
                , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));

            var modalDePermisos = new ModalDeConsultaDeRelaciones<PuestoDto, PermisosDeUnPuestoDto>(mantenimiento: Mnt
                              , tituloModal: "Permisos de un Puesto"
                              , crudModal: new DescriptorDePermisosDeUnPuesto(ModoDescriptor.Consulta)
                              , propiedadRestrictora: nameof(PermisosDeUnPuestoDto.IdPuesto));

            var mostrarPermisos = new ConsultarRelaciones(modalDePermisos.IdHtml, () => modalDePermisos.RenderControl());
            var opcion = new OpcionDeMenu<PuestoDto>(Mnt.ZonaMenu.Menu, mostrarPermisos, $"Permisos", enumModoDeAccesoDeDatos.Consultor, enumCssOpcionMenu.DeElemento);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestoDeTrabajo.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeTrabajo('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
