﻿using ModeloDeDto;
using ModeloDeDto.Seguridad;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeRolesDeUnPuesto : DescriptorDeCrud<RolesDeUnPuestoDto>
    {
        public DescriptorDeRolesDeUnPuesto(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(RolesDeUnPuestoController), nameof(RolesDeUnPuestoController.CrudRolesDeUnPuesto), modo, "Seguridad")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            new RestrictorDeFiltro<RolesDeUnPuestoDto>(bloque: fltGeneral
                  , etiqueta: "Puesto"
                  , propiedad:nameof(RolesDeUnPuestoDto.IdPuesto)
                  , ayuda: "buscar por puesto"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Rol", nameof(RolesDeUnPuestoDto.Rol), "Buscar por 'rol'");

            var modalDeRoles = new ModalDeRelacionarElementos<RolesDeUnPuestoDto, RolDto>(mantenimiento: Mnt
                              , tituloModal: "Seleccione los roles a relacionar"
                              , crudModal: new DescriptorDeRol(contexto,ModoDescriptor.Relacion)
                              , propiedadRestrictora: nameof(RolesDeUnPuestoDto.IdPuesto));

            var relacionarRoles = new RelacionarElementos(modalDeRoles.IdHtml, () => modalDeRoles.RenderControl(), "Añadir roles al puesto");
            var opcion = new OpcionDeMenu<RolesDeUnPuestoDto>(Mnt.ZonaMenu.Menu, relacionarRoles, $"Roles", enumModoDeAccesoDeDatos.Gestor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PermisosDeUnRolController)
                , vista: nameof(PermisosDeUnRolController.CrudPermisosDeUnRol)
                , relacionarCon: nameof(PermisoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PermisosDeUnRolDto>.NombreMnt
                , nombreOpcion: "Permisos"
                , propiedadQueRestringe: nameof(RolesDeUnPuestoDto.IdRol )
                , propiedadRestrictora: nameof(PermisosDeUnRolDto.IdRol)
                , "Incluir permisos en el rol seleccionado");

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/RolesDeUnPuesto.js¨></script>
                      <script>
                         try {{                           
                            Seguridad.CrearCrudDeRolesDeUnPuesto('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
