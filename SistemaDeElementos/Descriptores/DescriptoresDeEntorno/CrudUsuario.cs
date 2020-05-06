using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;
using Microsoft.AspNetCore.Hosting;
using MVCSistemaDeElementos.Controllers;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudUsuario : DescriptorDeCrud<UsuarioDto>
    {
        public CrudUsuario(ModoDescriptor modo)
        : base(controlador: nameof(UsuariosController), vista: $"{nameof(UsuariosController.CrudUsuario)}", modo: modo)
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
                       crudModal: new CrudPermiso(ModoDescriptor.Seleccion),
                       propiedadDondeMapear: FiltroPor.Nombre.ToString());
            
            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");            

            DefinirColumnasDelGrid();

            Mnt.Datos.ExpresionElemento = $"([{nameof(UsuarioDtm.Login)}]) [{nameof(UsuarioDtm.Apellido)}], [{nameof(UsuarioDtm.Nombre)}]";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/Usuarios.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }

    }
}
