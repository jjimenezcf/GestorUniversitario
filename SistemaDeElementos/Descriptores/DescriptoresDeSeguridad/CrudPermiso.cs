using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using MVCSistemaDeElementos.Controllers;
using SistemaDeElementos.Descriptores.Componentes.Elementos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudPermiso : DescriptorDeCrud<PermisoDto>
    {
        public CrudPermiso(ModoDescriptor modo)
        : base(controlador: nameof(PermisosController), vista: nameof(PermisosController.CrudPermiso), modo: modo)
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var modalUsuario = new CrudUsuario(ModoDescriptor.Seleccion);
                var filtrosEspeificos = new BloqueDeFitro<PermisoDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(2, 2));
                new SelectorDeFiltro<PermisoDto, UsuarioDto>(padre: filtrosEspeificos,
                                              etiqueta: "Usuario",
                                              filtrarPor: PermisoPor.PermisosDeUnUsuario,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDtm.Id),
                                              paraMostrar: nameof(UsuarioDtm.Apellido),
                                              crudModal: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

                new SelectorDeElemento<PermisoDto>(padre: filtrosEspeificos,
                                              etiqueta: "Clase",
                                              propiedad: nameof(PermisoDto.Clase) ,
                                              ayuda: "Seleccionar una clase",
                                              posicion: new Posicion() { fila = 0, columna = 1 },
                                              paraGuardarEn: nameof(PermisoDto.IdClase),
                                              claseElemento: nameof(ClasePermisoDto));
            }

            DefinirColumnasDelGrid();
            Mnt.Datos.ExpresionElemento = $"[{nameof(PermisoDto.Nombre)}]";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/Seguridad/Permisos.js¨></script>
                      <script>
                         Crud.crudMnt = new Seguridad.CrudMntPermiso('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }

    }
}
