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
                new SelectorDeFiltro<PermisoDto, UsuarioDto>(padre: new BloqueDeFitro<PermisoDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                              etiqueta: "Usuario",
                                              filtrarPor: PermisoPor.PermisoDeUnRol,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDtm.Id),
                                              paraMostrar: nameof(UsuarioDtm.Apellido),
                                              crudModal: modalUsuario,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
            }

            DefinirColumnasDelGrid();
        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Nombre), Titulo = "Nombre", Ordenar = true, PorAnchoMnt = 50 });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Clase), Titulo = "Clase", Tipo = typeof(int), PorAnchoMnt = 10 });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<PermisoDto> { Propiedad = nameof(PermisoDto.Permiso), Titulo = "Permiso", Tipo = typeof(int) });
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

                    fila.AnadirCelda(celda);
                }
                Mnt.Datos.AnadirFila(fila);
            }
        }

    }
}
