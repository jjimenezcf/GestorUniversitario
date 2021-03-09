using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto.Entorno;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePermiso : DescriptorDeCrud<PermisoDto>
    {
        public DescriptorDePermiso(ModoDescriptor modo)
        : base(controlador: nameof(PermisosController), vista: nameof(PermisosController.CrudPermiso), modo: modo, "Seguridad")
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var modalUsuario = new DescriptorDeUsuario(ModoDescriptor.Seleccion);
                var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
                var fltEspecificos = new BloqueDeFitro<PermisoDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2));
                
                new SelectorDeFiltro<PermisoDto, UsuarioDto>(padre: fltGeneral,
                                              etiqueta: "Usuario",
                                              filtrarPor: PermisoPor.PermisosDeUnUsuario,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 1 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.Apellido),
                                              crudModal: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

                new ListaDeElemento<PermisoDto>(padre: fltEspecificos,
                                                etiqueta: "Clase de permiso",
                                                ayuda:"selecciona una clase",
                                                seleccionarDe: nameof(ClasePermisoDto),
                                                filtraPor: nameof(PermisoDto.IdClase),
                                                mostrarExpresion: ClasePermisoDto.MostrarExpresion,
                                                posicion: new Posicion() { fila = 0, columna = 0 });
                
                new ListaDeElemento<PermisoDto>(padre: fltEspecificos,
                                                etiqueta: "Tipo de permiso",
                                                ayuda:"selecciona un tipo",
                                                seleccionarDe: nameof(TipoPermisoDto),
                                                filtraPor: nameof(PermisoDto.IdTipo),
                                                mostrarExpresion: nameof(TipoPermisoDto.Nombre),
                                                posicion: new Posicion() { fila = 1, columna = 0 });

                AnadirOpciondeRelacion(Mnt
                    , controlador: nameof(RolesDeUnPermisoController)
                    , vista: nameof(RolesDeUnPermisoController.CrudRolesDeUnPermiso)
                    , relacionarCon: nameof(RolDto)
                    , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPermisoDto>.NombreMnt
                    , nombreOpcion: "Roles"
                    , propiedadQueRestringe: nameof(PermisoDto.Id)
                    , propiedadRestrictora: nameof(PermisosDeUnRolDto.IdPermiso)
                    , "Añadir roles al permiso seleccionado");

            }
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Permisos.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePermisos('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
