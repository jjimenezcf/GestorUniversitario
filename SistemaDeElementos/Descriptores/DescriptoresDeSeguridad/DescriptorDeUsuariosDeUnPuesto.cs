using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeUsuariosDeUnPuesto : DescriptorDeCrud<UsuariosDeUnPuestoDto>
    {
        
        public DescriptorDeUsuariosDeUnPuesto(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(UsuariosDeUnPuestoController), nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto), modo, "Seguridad")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<UsuariosDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad:nameof(UsuariosDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por puesto"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Usuario", nameof(UsuariosDeUnPuestoDto.Usuario), "Buscar por 'usuario'");

            var modalDePuestos = new ModalDeRelacionarElementos<UsuariosDeUnPuestoDto, UsuarioDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los usuarios a relacionar"
                              , crudModal: new DescriptorDeUsuario(contexto,ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto));
            var relacionarPuestos = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl(), "Añadir usuarios al puesto");
            var opcion = new OpcionDeMenu<UsuariosDeUnPuestoDto>(Mnt.ZonaMenu.Menu, relacionarPuestos, $"Usuarios", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/UsuariosDeUnPuesto.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeUsuariosDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
