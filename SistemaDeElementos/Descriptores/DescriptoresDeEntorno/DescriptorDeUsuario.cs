using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto;
using GestorDeElementos;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Negocio;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeUsuario : DescriptorDeCrud<UsuarioDto>
    {
        DescriptorDePuestosDeUnUsuario descriptorDePuestos = new DescriptorDePuestosDeUnUsuario(ModoDescriptor.Mantenimiento);

        public DescriptorDeUsuario(ModoDescriptor modo)
        : base(controlador: nameof(UsuariosController)
               , vista: $"{nameof(UsuariosController.CrudUsuario)}"
               , modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var bloque = new BloqueDeFitro<UsuarioDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(3,2));
                new SelectorDeFiltro<UsuarioDto, PermisoDto>(
                       padre: bloque,
                       etiqueta: "Permiso",
                       filtrarPor: UsuariosPor.Permisos,
                       ayuda: "Seleccionar Permiso",
                       posicion: new Posicion(0,0),
                       paraFiltrar: nameof(PermisoDto.Id),
                       paraMostrar: nameof(PermisoDto.Nombre),
                       crudModal: new DescriptorDePermiso(ModoDescriptor.Seleccion),
                       propiedadDondeMapear: CamposDeFiltrado.Nombre.ToString());

                

                new ListasDinamicas<UsuarioDto>(padre: bloque,
                                                etiqueta: "Puesto de trabajo",
                                                filtrarPor: nameof(PuestosDeUnUsuarioDto.IdPuesto),
                                                ayuda: "usuarios de este puesto",
                                                seleccionarDe: nameof(PuestoDto),
                                                buscarPor: nameof(PuestoDto.Nombre),
                                                mostrarPropiedad: nameof(PuestoDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.contiene,
                                                posicion: new Posicion(1, 0)).LongitudMinimaParaBuscar = 1;


                new ListasDinamicas<UsuarioDto>(padre: bloque,
                                                etiqueta: "Roles",
                                                filtrarPor: nameof(RolesDeUnPuestoDto.IdRol),
                                                ayuda: "usuarios de un rol",
                                                seleccionarDe: nameof(RolDto),
                                                buscarPor: nameof(RolDto.Nombre),
                                                mostrarPropiedad: nameof(RolDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.contiene,
                                                posicion: new Posicion(2, 0)).LongitudMinimaParaBuscar = 1;



                new ListasDinamicas<UsuarioDto>(padre: bloque,
                                                etiqueta: "Permisos",
                                                filtrarPor: nameof(PermisosDeUnUsuarioDto.IdPermiso),
                                                ayuda: "permisos de un usuario",
                                                seleccionarDe: nameof(PermisoDto),
                                                buscarPor: nameof(PermisoDto.Nombre),
                                                mostrarPropiedad: nameof(PermisoDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.comienza,
                                                posicion: new Posicion(3, 0)).LongitudMinimaParaBuscar = 3;

            }
            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Usuario",UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");
            RutaVista = "Entorno";

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PuestosDeUnUsuarioController)
                , vista: nameof(PuestosDeUnUsuarioController.CrudPuestosDeUnUsuario)
                , relacionarCon: nameof(PuestoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PuestosDeUnUsuarioDto>.NombreMnt
                , nombreOpcion: "Puestos"
                , propiedadQueRestringe: nameof(UsuarioDto.Id)
                , propiedadRestrictora: nameof(PuestosDeUnUsuarioDto.IdUsuario));

            var modalDePermisos = new ModalDeConsultaDeRelaciones<UsuarioDto, PermisosDeUnUsuarioDto>(mantenimiento: Mnt
                              , tituloModal: "Permisos de un usuario"
                              , crudModal: new DescriptorDePermisosDeUnUsuario(ModoDescriptor.Consulta)
                              , propiedadRestrictora: nameof(PermisosDeUnUsuarioDto.IdUsuario));

            var mostrarPermisos = new ConsultarRelaciones(modalDePermisos.IdHtml, () => modalDePermisos.RenderControl());
            var opcion = new OpcionDeMenu<UsuarioDto>(Mnt.ZonaMenu.Menu, mostrarPermisos, $"Permisos", enumModoDeAccesoDeDatos.Consultor, enumCssOpcionMenu.DeElemento);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Entorno/Usuario.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudDeUsuarios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
