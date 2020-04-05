using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudPermiso : DescriptorDeCrud<PermisoDto>
    {
        public CrudPermiso(ModoDescriptor modo)
        : base(controlador: "Permisos", vista: "MantenimientoPermiso", titulo: "Mantenimiento de permisos", modo: modo)
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var modalUsuario = new CrudUsuario(ModoDescriptor.Seleccion);
                new SelectorDeFiltro<PermisoDto, UsuarioDto>(padre: new BloqueDeFitro<PermisoDto>(filtro: DescriptorDeMantenimiento.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                              etiqueta: "Usuario",
                                              propiedad: PermisoPor.PermisoDeUnRol,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDtm.Id),
                                              paraMostrar: nameof(UsuarioDtm.Apellido),
                                              descriptor: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
            }

            DefinirVistaDeCreacion(accion: "IraCrearPermiso", textoMenu: "Crear permiso");

            DefinirColumnasDelGrid();

        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            DescriptorDeMantenimiento.Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Id), Visible = false, Tipo = typeof(int) });
            DescriptorDeMantenimiento.Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Nombre), Titulo = "Nombre", Ordenar = false });
            DescriptorDeMantenimiento.Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Clase), Titulo = "Clase", Tipo = typeof(int) });
            DescriptorDeMantenimiento.Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Permiso), Titulo = "Permiso", Tipo = typeof(int) });
        }

        public override void MapearElementosAlGrid(IEnumerable<PermisoDto> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var permiso in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in DescriptorDeMantenimiento.Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(PermisoDto.Id))
                        celda.Valor = permiso.Id.ToString();
                    else
                    if (columna.Nombre == nameof(PermisoDto.Nombre))
                        celda.Valor = permiso.Nombre;
                    else
                    if (columna.Nombre == nameof(PermisoDto.Clase))
                        celda.Valor = permiso.Clase;
                    else
                    if (columna.Nombre == nameof(PermisoDto.Permiso))
                        celda.Valor = permiso.Permiso;

                    fila.Celdas.Add(celda);
                }
                DescriptorDeMantenimiento.Grid.Filas.Add(fila);
            }
        }

    }
}
