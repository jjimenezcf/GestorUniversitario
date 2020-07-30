using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestoDeTrabajo : DescriptorDeCrud<PuestoDto>
    {
        public DescriptorDePuestoDeTrabajo(ModoDescriptor modo)
            : base(nameof(PuestoDeTrabajoController), nameof(PuestoDeTrabajoController.CrudPuestoDeTrabajo), modo)
        {
            RutaVista = "Seguridad";
            AnadirOpcionDeRolesDeUnPuesto($"{Mnt.MenuDeMnt.Menu.IdHtml}-{nameof(RolDto)}");
        }

        internal void AnadirOpcionDeRolesDeUnPuesto(string idForm)
        {
            var mntRoles = new AccionDeNavegarParaRelacionar(
                    urlDelCrud: $@"/{nameof(RolesDeUnPuestoController).Replace("Controller", "")}/{nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)}"
                  , relacionarCon: nameof(RolDto)
                  , nombreDelMnt: DescriptorMantenimiento<RolesDeUnPuestoDto>.nombreMnt
                  , idForm: idForm);
            var opcion = new OpcionDeMenu<PuestoDto>(menu: Mnt.MenuDeMnt.Menu, accion: mntRoles, tipoAccion: TipoAccion.Post, titulo: $"Roles");
            Mnt.MenuDeMnt.Menu.Add(opcion);
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
