using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Elementos.Seguridad;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;
using Gestor.Elementos.Entorno;
using ServicioDeDatos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudPermiso : DescriptorDeCrud<PermisoDto>
    {
        public CrudPermiso(ModoDescriptor modo)
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

                new SelectorDeElemento<PermisoDto>(padre: fltEspecificos,
                                              propiedad: nameof(PermisoDto.Clase) ,
                                              posicion: new Posicion() { fila = 0, columna = 0 });
                
                new SelectorDeElemento<PermisoDto>(padre: fltEspecificos,
                                        propiedad: nameof(PermisoDto.Tipo),
                                        posicion: new Posicion() { fila = 0, columna = 1 });
            }

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
