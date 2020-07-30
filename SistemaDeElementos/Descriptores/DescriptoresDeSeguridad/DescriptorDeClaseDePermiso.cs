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
                         try {{                           
                            Seguridad.CrearCrudDeClasesDePermiso('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
