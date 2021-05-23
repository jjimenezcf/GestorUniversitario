using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestosDeUnRol : DescriptorDeCrud<PuestosDeUnRolDto>
    {
        public DescriptorDePuestosDeUnRol(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(PuestosDeUnRolController), nameof(PuestosDeUnRolController.CrudPuestosDeUnRol), modo, "Seguridad")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestosDeUnRolDto>(bloque: fltGeneral
                  , etiqueta: "Rol"
                  , propiedad:nameof(PuestosDeUnRolDto.IdRol)
                  , ayuda: "buscar por rol"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Puestos", nameof(PuestosDeUnRolDto.Puesto), "Buscar por 'puesto'");

            var modalDePuestos = new ModalDeRelacionarElementos<PuestosDeUnRolDto, PuestoDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los puestos a relacionar"
                              , crudModal: new DescriptorDePuestoDeTrabajo(contexto, ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(PuestosDeUnRolDto.IdRol));

            var relacionarRoles = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl(), "Seleccionar los puestos que han de tener un rol");
            var opcion = new OpcionDeMenu<PuestosDeUnRolDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Puestos", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(UsuariosDeUnPuestoController)
                , vista: nameof(UsuariosDeUnPuestoController.CrudUsuariosDeUnPuesto)
                , relacionarCon: nameof(UsuarioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<UsuariosDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Usuarios"
                , propiedadQueRestringe: nameof(UsuariosDeUnPuestoDto.IdPuesto)
                , propiedadRestrictora: nameof(UsuariosDeUnPuestoDto.IdPuesto)
                , "Incluir usuarios en el puesto seleccionado");
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/PuestosDeUnRol.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeUnRol('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
