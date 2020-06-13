using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Seguridad;
using MVCSistemaDeElementos.Controllers;
using MVCSistemaDeElementos.Descriptores;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeClaseDePermiso : DescriptorDeCrud<ClasePermisoDto>
    {
        public DescriptorDeClaseDePermiso(ModoDescriptor modo)
            : base(nameof(ClaseDePermisoController), nameof(ClaseDePermisoController.CrudClaseDePermiso), modo)
        {
            RutaVista = "Seguridad";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/ClaseDePermiso.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntClaseDePermiso('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }

    }
}
