﻿using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto;
using GestorDeElementos;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Negocio;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeUsuario : DescriptorDeCrud<UsuarioDto>
    {
       
        public DescriptorDeUsuario(ContextoSe contexto, ModoDescriptor modo): this(contexto, modo, null)
        {

        }
        public DescriptorDeUsuario(ContextoSe contexto, ModoDescriptor modo, string id)
        : base(contexto,
              controlador: nameof(UsuariosController)
               , vista: $"{nameof(UsuariosController.CrudUsuario)}"
               , modo: modo
              , rutaBase: "Entorno"
              , id)
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
                       crudModal: new DescriptorDePermiso(Contexto, ModoDescriptor.SeleccionarParaFiltrar),
                       propiedadDondeMapear: ltrFiltros.Nombre.ToString());                

                new ListasDinamicas<UsuarioDto>(bloque: bloque,
                                                etiqueta: "Puesto de trabajo",
                                                filtrarPor: nameof(PuestosDeUnUsuarioDto.IdPuesto),
                                                ayuda: "usuarios de este puesto",
                                                seleccionarDe: nameof(PuestoDto),
                                                buscarPor: nameof(PuestoDto.Nombre),
                                                mostrarExpresion: nameof(PuestoDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.contiene,
                                                posicion: new Posicion(1, 0),
                                                controlador: nameof(PuestoDeTrabajoController),
                                                restringirPor: "").LongitudMinimaParaBuscar = 1;


                new ListasDinamicas<UsuarioDto>(bloque: bloque,
                                                etiqueta: "Roles",
                                                filtrarPor: nameof(RolesDeUnPuestoDto.IdRol),
                                                ayuda: "usuarios de un rol",
                                                seleccionarDe: nameof(RolDto),
                                                buscarPor: nameof(RolDto.Nombre),
                                                mostrarExpresion: nameof(RolDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.contiene,
                                                posicion: new Posicion(2, 0),
                                                controlador: nameof(RolController),
                                                restringirPor: "").LongitudMinimaParaBuscar = 1;



                new ListasDinamicas<UsuarioDto>(bloque: bloque,
                                                etiqueta: "Permisos",
                                                filtrarPor: nameof(PermisosDeUnUsuarioDto.IdPermiso),
                                                ayuda: "permisos de un usuario",
                                                seleccionarDe: nameof(PermisoDto),
                                                buscarPor: nameof(PermisoDto.Nombre),
                                                mostrarExpresion: nameof(PermisoDto.Nombre),
                                                criterioDeBusqueda: CriteriosDeFiltrado.comienza,
                                                posicion: new Posicion(3, 0),
                                                controlador: nameof(PermisosController),
                                                restringirPor: "").LongitudMinimaParaBuscar = 3;

            }
            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Usuario",UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");

            var modalDePermisos = new ModalDeConsultaDeRelaciones<UsuarioDto, PermisosDeUnUsuarioDto>(mantenimiento: Mnt
                              , tituloModal: "Permisos de un usuario"
                              , crudModal: new DescriptorDePermisosDeUnUsuario(Contexto, ModoDescriptor.Consulta)
                              , propiedadRestrictora: nameof(PermisosDeUnUsuarioDto.IdUsuario));

            var mostrarPermisos = new ConsultarRelaciones(modalDePermisos.IdHtml, () => modalDePermisos.RenderControl(), "Mostrar los permisos de un usuario");
            var opcion = new OpcionDeMenu<UsuarioDto>(Mnt.ZonaMenu.Menu, mostrarPermisos, $"Permisos", enumModoDeAccesoDeDatos.Consultor);
            Mnt.ZonaMenu.Menu.Add(opcion);

            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(PuestosDeUnUsuarioController)
                , vista: nameof(PuestosDeUnUsuarioController.CrudPuestosDeUnUsuario)
                , relacionarCon: nameof(PuestoDto)
                , navegarAlCrud: DescriptorDeMantenimiento<PuestosDeUnUsuarioDto>.NombreMnt
                , nombreOpcion: "Puestos"
                , propiedadQueRestringe: nameof(UsuarioDto.Id)
                , propiedadRestrictora: nameof(PuestosDeUnUsuarioDto.IdUsuario)
                , "Añadir puestos al usuario seleccionado");

            Mnt.OrdenacionInicial = @$"{nameof(UsuarioDto.NombreCompleto)}:nombre:{enumModoOrdenacion.ascendente.Render()}";
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();
            
            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Usuario.js¨></script>
                      <script>
                         try {{                           
                           Entorno.CrearCrudDeUsuarios('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
