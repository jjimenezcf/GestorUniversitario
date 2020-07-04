using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRolesDeUnPuesto : DescriptorDeCrud<RolesDeUnPuestoDto>
    {
        public DescriptorDeRolesDeUnPuesto(ModoDescriptor modo)
        : base(nameof(RolesDeUnPuestoController), nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<RolesDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad:nameof(RolesDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por puesto"
                  , new Posicion { fila = 0, columna = 0 });
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/RolesDeUnPuesto.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntRolesDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }
    }
}
