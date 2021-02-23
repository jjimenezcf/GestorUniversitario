using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePermisosDeUnRol : DescriptorDeCrud<PermisosDeUnRolDto>
    {
        public DescriptorDePermisosDeUnRol(ModoDescriptor modo)
        : base(nameof(PermisosDeUnRolController), nameof(PermisosDeUnRolController.CrudPermisosDeUnRol), modo, "Seguridad")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta("General");
            new RestrictorDeFiltro<PermisosDeUnRolDto>(bloque: fltGeneral
                  , etiqueta: "Rol"
                  , propiedad: nameof(PermisosDeUnRolDto.IdRol)
                  , ayuda: "buscar por rol"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(CamposDeFiltrado.Nombre).CambiarAtributos("Permiso", nameof(PermisosDeUnRolDto.Permiso), "Buscar por 'permiso'");

            //Añade una opcion de menú, para relacionar permisos
            //- Abre una modal de selección
            //- Le pasa el id del elemento con el que se va a relacionar (para no mostrar los ya relacionados)
            //- Al aceptar --> llama al negocio y relaciona los id's 
            //- Al cerrar no hace nada
            var modalDePermisos =  new ModalDeRelacionarElementos<PermisosDeUnRolDto, PermisoDto>(mantenimiento: Mnt
                  ,tituloModal: "Seleccione los permisos a relacionar"
                  ,crudModal: new DescriptorDePermiso(ModoDescriptor.Relacion)
                  ,propiedadRestrictora: nameof(PermisosDeUnRolDto.IdRol));

            var relacionarPermisos = new RelacionarElementos(modalDePermisos.IdHtml, () => modalDePermisos.RenderControl(), "Seleccionar permisos a relacionar con el rol");
            var opcion = new OpcionDeMenu<PermisosDeUnRolDto>(Mnt.ZonaMenu.Menu, relacionarPermisos, $"Permisos", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/PermisosDeUnRol.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDePermisosDeUnRol('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
