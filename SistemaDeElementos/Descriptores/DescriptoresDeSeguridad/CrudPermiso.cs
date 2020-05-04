using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudPermiso : DescriptorDeCrud<PermisoDto>
    {
        public CrudPermiso(ModoDescriptor modo)
        : base(controlador: "Permisos", vista: "MantenimientoPermiso", elemento: "Permiso", modo: modo)
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
                //new SelectorDeTabla<PermisoDto, ClasePermisoDto>(Padre: filtrosEspeificos,
                //                              etiqueta: "Clase",
                //                              filtrarPor: PermisoPor.ClaseDePermisos,
                //                              ayuda: "Seleccionar una clase",
                //                              posicion: new Posicion() { fila = 1, columna = 0 },
                //                              paraFiltrar: nameof(ClasePermisoDto.Id),
                //                              paraMostrar: nameof(ClasePermisoDto.Nombre),
                //                              accion: accionBuscarClase);
            }

            DefinirColumnasDelGrid();
        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Nombre), Titulo = "Nombre", Ordenar = true, PorAnchoMnt = 50 });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Clase), Titulo = "Clase", PorAnchoMnt = 20 });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Permiso), Titulo = "Permiso" });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.IdClase), Tipo = typeof(int), Visible = false });
            Mnt.Datos.ExpresionElemento = $"[{nameof(PermisoDto.Nombre)}]";
        }

        public override void MapearElementosAlGrid(IEnumerable<PermisoDto> elementos, int cantidadPorLeer, int posicionInicial)
        {
            base.MapearElementosAlGrid(elementos, cantidadPorLeer, posicionInicial);
            foreach (var permiso in elementos)
            {
                var fila = new FilaDelGrid<PermisoDto>(Mnt.Datos, permiso);
                foreach (ColumnaDelGrid<PermisoDto> columna in Mnt.Datos.Columnas)
                {
                    CeldaDelGrid<PermisoDto> celda = new CeldaDelGrid<PermisoDto>(columna);
                    if (columna.Propiedad == nameof(PermisoDto.Nombre))
                        celda.Valor = permiso.Nombre;
                    else
                    if (columna.Propiedad == nameof(PermisoDto.Clase))
                        celda.Valor = permiso.Clase;
                    else
                    if (columna.Propiedad == nameof(PermisoDto.Permiso))
                        celda.Valor = permiso.Permiso;
                    else
                    if (columna.Propiedad == nameof(PermisoDto.IdClase))
                        celda.Valor = permiso.IdClase;

                    fila.AnadirCelda(celda);
                }
                Mnt.Datos.AnadirFila(fila);
            }
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
