using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePuestosDeUnUsuario : DescriptorDeCrud<PuestosDeUnUsuarioDto>
    {
        
        public DescriptorDePuestosDeUnUsuario(ModoDescriptor modo)
        : base(nameof(PuestosDeUnUsuarioController), nameof(PuestosDeUnUsuarioController.CrudPuestosDeUnUsuario), modo, "Seguridad")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PuestosDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad:nameof(PuestosDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Puesto", nameof(PuestosDeUnUsuarioDto.Puesto), "Buscar por 'puesto'");


            var modalDePuestos = new ModalDeRelacionarElementos<PuestosDeUnUsuarioDto, PuestoDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los puestos a relacionar"
                              , crudModal: new DescriptorDePuestoDeTrabajo(ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(PuestosDeUnUsuarioDto.IdUsuario));
            var relacionarPuestos = new RelacionarElementos(modalDePuestos.IdHtml, () => modalDePuestos.RenderControl(), "Seleccionar los puestos de un usuario");
            var opcion = new OpcionDeMenu<PuestosDeUnUsuarioDto>(Mnt.ZonaMenu.Menu, relacionarPuestos, $"Puestos", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(RolesDeUnPuestoController)
                , vista: nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto)
                , relacionarCon: nameof(RolDto)
                , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPuestoDto>.NombreMnt
                , nombreOpcion: "Roles"
                , propiedadQueRestringe: nameof(PuestosDeUnUsuarioDto.IdPuesto)
                , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto)
                , ayuda: "Añadir roles al puesto seleccionado");
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/PuestosDeUnUsuario.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePuestosDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el crud', error);
                         }}
                      </script>
                    ";
            return render.Render();
        }
    }
}
