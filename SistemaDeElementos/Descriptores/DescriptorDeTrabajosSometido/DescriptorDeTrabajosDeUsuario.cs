using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrabajosDeUsuario : DescriptorDeCrud<TrabajoDeUsuarioDto>
    {
        public class IniciarTrabajo : AccionDeMenuMnt
        {
            const string iniciar = "IniciarTrabajo";
            public IniciarTrabajo()
            : base(iniciar,enumCssOpcionMenu.DeElemento, "Inicia un trabajo")
            {
            }
        }
        public class BloquearTrabajo : AccionDeMenuMnt
        {
            const string bloquear = "BloquearTrabajo";
            public BloquearTrabajo()
            : base(bloquear, enumCssOpcionMenu.DeElemento, "Bloquear un trabajo")
            {
            }
        }

        public DescriptorDeTrabajosDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(TrabajosDeUsuarioController)
               , vista: $"{nameof(TrabajosDeUsuarioController.CrudDeTrabajosDeUsuario)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
            var iniciarTrabajo = new IniciarTrabajo();
            var opcion = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, iniciarTrabajo, $"Iniciar", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            var bloquearTrabajo = new BloquearTrabajo();
            var opcionBloquear = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, bloquearTrabajo, $"Bloquear", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionBloquear);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/TrabajosDeUsuario.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeTrabajosDeUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
