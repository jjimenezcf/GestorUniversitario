using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRol : DescriptorDeCrud<RolDto>
    {
        public DescriptorDeRol(ModoDescriptor modo)
            : base(nameof(RolController), nameof(RolController.CrudRol), modo)
        {
            RutaVista = "Seguridad";

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PuestosDeUnRolController)
                , vista: nameof(PuestosDeUnRolController.CrudPuestosDeUnRol)
                , relacionarCon: nameof(PuestoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PuestosDeUnRolDto>.NombreMnt
                , nombreOpcion: "Puestos"
                , propiedadQueRestringe: nameof(RolDto.Id)
                , propiedadRestrictora: nameof(PuestosDeUnRolDto.IdRol)
                , "Incluir el rol a los puestos seleccionados");

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PermisosDeUnRolController)
                , vista: nameof(PermisosDeUnRolController.CrudPermisosDeUnRol)
                , relacionarCon: nameof(PermisoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PermisosDeUnRolDto>.NombreMnt
                , nombreOpcion: "Permisos"
                , propiedadQueRestringe: nameof(RolDto.Id)
                , propiedadRestrictora: nameof(PermisosDeUnRolDto.IdRol)
                , "Añadir permisos al rol seleccionado");


            var bloque = new BloqueDeFitro<RolDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2));
            new ListasDinamicas<RolDto>(bloque: bloque,
                                            etiqueta: "Permisos asociado",
                                            filtrarPor: nameof(PermisosDeUnRolDto.IdPermiso),
                                            ayuda: "roles con el permiso",
                                            seleccionarDe: nameof(PermisoDto),
                                            buscarPor: nameof(PermisoDto.Nombre),
                                            mostrarExpresion: nameof(PermisoDto.Nombre),
                                            criterioDeBusqueda: CriteriosDeFiltrado.contiene,
                                            posicion: new Posicion(1, 0)).LongitudMinimaParaBuscar = 3;
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/Rol.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeRoles('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
