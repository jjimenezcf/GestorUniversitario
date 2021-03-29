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
            : base(nameof(PuestoDeTrabajoController), nameof(PuestoDeTrabajoController.CrudPuestoDeTrabajo), modo, "Seguridad")
        {
            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(UsuariosDeUnPuestoController)
                , vista: nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto)
                , relacionarCon: nameof(UsuarioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<UsuariosDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Usuarios"
                , propiedadQueRestringe: nameof(PuestoDto.Id)
                , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto)
                , "Incluir usuarios en el puesto seleccionado");

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(RolesDeUnPuestoController)
                , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
                , relacionarCon: nameof(RolDto)
                , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Roles"
                , propiedadQueRestringe: nameof(PuestoDto.Id)
                , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto)
                , "Añadir roles al puesto seleccionado");

            var modalDePermisos = new ModalDeConsultaDeRelaciones<PuestoDto, PermisosDeUnPuestoDto>(mantenimiento: Mnt
                              , tituloModal: "Permisos de un Puesto"
                              , crudModal: new DescriptorDePermisosDeUnPuesto(ModoDescriptor.Consulta)
                              , propiedadRestrictora: nameof(PermisosDeUnPuestoDto.IdPuesto));

            var mostrarPermisos = new ConsultarRelaciones(modalDePermisos.IdHtml, () => modalDePermisos.RenderControl(), "Mostrar los permisos de un puesto de trabajo");
            var opcion = new OpcionDeMenu<PuestoDto>(Mnt.ZonaMenu.Menu, mostrarPermisos, $"Permisos", enumModoDeAccesoDeDatos.Consultor);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/PuestoDeTrabajo.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeTrabajo('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
