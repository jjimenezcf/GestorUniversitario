using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestoDeUnUsuario : DescriptorDeCrud<PuestoDeUnUsuarioDto>
    {
        public DescriptorDePuestoDeUnUsuario(ModoDescriptor modo)
            : base(nameof(PuestoDeUnUsuarioController), nameof(PuestoDeUnUsuarioController.CrudPuestoDeUnUsuario), modo)
        {
            RutaVista = "Seguridad";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestoDeUnUsuario.js¨></script>
                      <script>
                         Crud.crudMnt = new Entorno.CrudMntPuestoDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }
    }
}
