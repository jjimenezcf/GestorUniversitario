using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeUsuariosDeUnPuesto : DescriptorDeCrud<UsuariosDeUnPuestoDto>
    {
        
        public DescriptorDeUsuariosDeUnPuesto(ModoDescriptor modo)
        : base(nameof(UsuariosDeUnPuestoController), nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<UsuariosDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad:nameof(UsuariosDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por puesto"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos("Usuario", nameof(UsuariosDeUnPuestoDto.Usuario), "Buscar por 'usuario'");

            var modalDePuestos = new ModalDeRelacionarElementos<UsuariosDeUnPuestoDto, UsuarioDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los usuarios a relacionar"
                              , crudModal: new DescriptorDeUsuario(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto));
            var relacionarPuestos = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl());
            var opcion = new OpcionDeMenu<UsuariosDeUnPuestoDto>(Mnt.ZonaMenu.Menu, relacionarPuestos, $"Usuarios");
            Mnt.ZonaMenu.Menu.Add(opcion);

            //AnadirOpciondeRelacion(Mnt
            //    , controlador: nameof(RolesDeUnPuestoController)
            //    , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
            //    , relacionarCon: nameof(RolDto)
            //    , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.NombreMnt
            //    , nombreOpcion: "Roles"
            //    , propiedadQueRestringe: nameof(UsuariosDeUnPuestoDto.IdPuesto)
            //    , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/UsuariosDeUnPuesto.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeUsuariosDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
