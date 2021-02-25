using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeTrabajosDeUsuario : DescriptorDeCrud<TrabajoDeUsuarioDto>
    {
        public class AccionesDeTu : AccionDeMenuMnt
        {
            const string desbloquear = "desbloquear-trabajo";
            const string bloquear = "bloquear-trabajo";
            const string iniciar = "iniciar-trabajo";
            public AccionesDeTu(string accion, string ayuda)
            : base(accion, enumCssOpcionMenu.DeElemento, ayuda)
            {
            }

            public static AccionesDeTu Desbloquear => new AccionesDeTu(desbloquear, "Desbloquear un trabajo");
            public static AccionesDeTu Bloquear => new AccionesDeTu(bloquear, "Bloquear un trabajo");
            public static AccionesDeTu Iniciar => new AccionesDeTu(iniciar, "Ejecutar un trabajo");

            public override string RenderAccion()
            {
                return $"javascript:TrabajosSometido.Eventos('{TipoDeAccion}','')";
            }
        }

        public DescriptorDeTrabajosDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(TrabajosDeUsuarioController)
               , vista: $"{nameof(TrabajosDeUsuarioController.CrudDeTrabajosDeUsuario)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
            var opcion = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Iniciar, $"Ejecutar", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            var opcionBloquear = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Bloquear, $"Bloquear", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionBloquear);

            var opcionDesbloquear = new OpcionDeMenu<TrabajoDeUsuarioDto>(Mnt.ZonaMenu.Menu, AccionesDeTu.Desbloquear, $"Desbloquear", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcionDesbloquear);
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
