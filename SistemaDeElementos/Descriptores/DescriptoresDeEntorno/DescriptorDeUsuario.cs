using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using ServicioDeDatos;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;
using Microsoft.AspNetCore.Hosting;
using MVCSistemaDeElementos.Controllers;
using Gestor.Elementos.Entorno;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using GestorDeSeguridad.ModeloIu;

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
                new SelectorDeFiltro<UsuarioDto, PermisoDto>(
                       padre: new BloqueDeFitro<UsuarioDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                       etiqueta: "Permiso",
                       filtrarPor: UsuariosPor.Permisos,
                       ayuda: "Seleccionar Permiso",
                       posicion: new Posicion() { fila = 0, columna = 0 },
                       paraFiltrar: nameof(PermisoDto.Id),
                       paraMostrar: nameof(PermisoDto.Nombre),
                       crudModal: new DescriptorDePermiso(ModoDescriptor.Seleccion),
                       propiedadDondeMapear: FiltroPor.Nombre.ToString());
            
            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");
            Mnt.Datos.ExpresionElemento = $"([{nameof(UsuarioDtm.Login)}]) [{nameof(UsuarioDtm.Apellido)}], [{nameof(UsuarioDtm.Nombre)}]";
            RutaVista = "Entorno";

            AnadirOpcionDePuestosDeUnUsuario($"{Mnt.MenuDeMnt.Menu.IdHtml}-{nameof(PuestoDto)}");
        }

        internal void AnadirOpcionDePuestosDeUnUsuario(string idForm)
        {
            var mntPuestos = new AccionDeNavegarParaRelacionar(TipoAccionMnt.RelacionarElementos
                  , $@"/{nameof(PuestosDeUnUsuarioController).Replace("Controller", "")}/{nameof(PuestosDeUnUsuarioController.CrudPuestoDeUnUsuario)}"
                  , nameof(PuestoDto)
                  , idForm);
            var opcion = new OpcionDeMenu<UsuarioDto>(menu: Mnt.MenuDeMnt.Menu, accion: mntPuestos, tipoAccion: TipoAccion.Post, titulo: $"Puestos");
            Mnt.MenuDeMnt.Menu.Add(opcion);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Entorno/CrudDeUsuario.js¨></script>
                      <script>
                         Crud.crudMnt = new Entorno.CrudMntUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }

    }
}
