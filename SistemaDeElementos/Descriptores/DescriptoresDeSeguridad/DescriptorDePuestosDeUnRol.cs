using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestosDeUnRol : DescriptorDeCrud<PuestosDeUnRolDto>
    {
        public DescriptorDePuestosDeUnRol(ModoDescriptor modo)
        : base(nameof(PuestosDeUnRolController), nameof(PuestosDeUnRolController.CrudPuestosDeUnRol), modo)
        {
            RutaVista = "Seguridad";
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestosDeUnRolDto>(bloque: fltGeneral
                  , etiqueta: "Rol"
                  , propiedad:nameof(PuestosDeUnRolDto.IdRol)
                  , ayuda: "buscar por rol"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos("Puestos", nameof(PuestosDeUnRolDto.Puesto), "Buscar por 'puesto'");

            var modalDePuestos = new ModalDeRelacionarElementos<PuestosDeUnRolDto, PuestoDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los puestos a relacionar"
                              , crudModal: new DescriptorDePuestoDeTrabajo(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(PuestosDeUnRolDto.IdRol));

            var relacionarRoles = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl());
            var opcion = new OpcionDeMenu<PuestosDeUnRolDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Puestos", enumModoDeAccesoDeDatos.Gestor, enumCssOpcionMenu.DeVista);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(UsuariosDeUnPuestoController)
                , vista: nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto)
                , relacionarCon: nameof(UsuarioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<UsuariosDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Usuarios"
                , propiedadQueRestringe: nameof(UsuariosDeUnPuestoDto.IdPuesto)
                , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto));
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/PuestosDeUnRol.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeUnRol('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
