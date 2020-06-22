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
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestoDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad:nameof(PuestoDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestoDeUnUsuario.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntPuestoDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }
    }
}
