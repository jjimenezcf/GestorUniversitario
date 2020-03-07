using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Permiso;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudCurso : DescriptorDeCrud<PermisoDto>
    {
        public CrudCurso(ModoDescriptor modo)
        : base(controlador: "Cursos", vista: "MantenimientoCurso", titulo: "Mantenimiento de cursos", modo: modo)
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var descEstu = new CrudUsuario(ModoDescriptor.Seleccion);
                new Selector<UsuarioDto>(padre: new Bloque(Filtro, "Específico", new Dimension(1, 2)),
                                              etiqueta: "Usuario",
                                              propiedad: PermisoPor.PermisoDeUnRol,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.Apellido),
                                              descriptor: descEstu,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
            }

            DefinirVistaDeCreacion(accion: "IraCrearCurso", textoMenu: "Crear curso");

            DefinirColumnasDelGrid();

        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Id), Visible = false, Tipo = typeof(int) });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Nombre), Titulo = "Título", Ordenar = false });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(PermisoDto.Clase), Titulo = "Créditos", Tipo = typeof(int) });
        }

        public override void MapearElementosAlGrid(IEnumerable<PermisoDto> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var curso in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(PermisoDto.Id))
                        celda.Valor = curso.Id.ToString();
                    else
                    if (columna.Nombre == nameof(PermisoDto.Nombre))
                        celda.Valor = curso.Nombre;
                    else
                    if (columna.Nombre == nameof(PermisoDto.Clase))
                        celda.Valor = curso.Clase;
                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
