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
        : base(controlador: nameof(PermisosController), vista: nameof(PermisosController.CrudPermiso), modo: modo)
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var modalUsuario = new DescriptorDeUsuario(ModoDescriptor.Seleccion);
                var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
                var fltEspecificos = new BloqueDeFitro<PermisoDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 4));
                
                //var fltRelacionados = new BloqueDeFitro<PermisoDto>(filtro: Mnt.Filtro, titulo: "Relacionados", dimension: new Dimension(1, 2));
                new SelectorDeFiltro<PermisoDto, UsuarioDto>(padre: fltGeneral,
                                              etiqueta: "Usuario",
                                              filtrarPor: PermisoPor.PermisosDeUnUsuario,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 1 },
                                              paraFiltrar: nameof(UsuarioDtm.Id),
                                              paraMostrar: nameof(UsuarioDtm.Apellido),
                                              crudModal: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

                new ListaDeElemento<PermisoDto>(padre: fltEspecificos,
                                              propiedad: nameof(PermisoDto.Clase) ,
                                              posicion: new Posicion() { fila = 0, columna = 0 });
                
                new ListaDeElemento<PermisoDto>(padre: fltEspecificos,
                                        propiedad: nameof(PermisoDto.Tipo),
                                        posicion: new Posicion() { fila = 0, columna = 1 });

                AnadirOpciondeRelacion(Mnt
                    , controlador: nameof(RolesDeUnPermisoController)
                    , vista: nameof(RolesDeUnPermisoController.CrudRolesDeUnPermiso)
                    , relacionarCon: nameof(RolDto)
                    , navegarAlCrud: DescriptorDeMantenimiento<RolesDeUnPermisoDto>.NombreMnt
                    , nombreOpcion: "Roles"
                    , propiedadQueRestringe: nameof(PermisoDtm.Id)
                    , propiedadRestrictora: nameof(PermisosDeUnRolDtm.IdPermiso));

            }

            RutaVista = "Seguridad";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/Permisos.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePermisos('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
