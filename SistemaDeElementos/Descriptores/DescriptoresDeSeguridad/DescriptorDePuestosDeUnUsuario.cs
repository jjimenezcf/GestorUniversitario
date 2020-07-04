using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestosDeUnUsuario : DescriptorDeCrud<PuestosDeUnUsuarioDto>
    {
        public DescriptorDePuestosDeUnUsuario(ModoDescriptor modo)
        : base(nameof(PuestosDeUnUsuarioController), nameof(PuestosDeUnUsuarioController.CrudPuestoDeUnUsuario), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestosDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad:nameof(PuestosDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestosDeUnUsuario.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntPuestosDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }
    }
}
