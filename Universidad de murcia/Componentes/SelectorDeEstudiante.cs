using AutoMapper;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Collections.Generic;
using UtilidadesParaIu;

namespace Componentes
{
    public class SelectorDeEstudiante
    {
        public SelectorModal Selector { get; }

        private GestorDeEstudiantes _gestordeEstudiantes;

        public SelectorDeEstudiante(ContextoUniversitario contexto, IMapper mapeador)
        {
            _gestordeEstudiantes = new GestorDeEstudiantes(contexto, mapeador);
            Selector = new SelectorModal(nameof(ElementoEstudiante), RenderizarTabla)
            {
                ColumnaId = nameof(ElementoEstudiante.Id),
                ColumnaMostrar = nameof(ElementoEstudiante.Apellido)
            };
        }


        public string RenderizarTabla()
        {
            var descriptorColumnas = DefinirColumnasGrid();
            var filas = ObtenerFilasDelGrid(descriptorColumnas);

            Grid grid = new Grid(Selector.IdGrig, descriptorColumnas, filas.filas)
            {
                Ruta = "Estudiantes",
                TotalEnBd = filas.totalBD,
                ConNavegador = true,
                ConSeleccion = true
            };

            return grid.ToHtml();
        }

        private (List<FilaDelGrid> filas,int totalBD) ObtenerFilasDelGrid(List<ColumnaDelGrid> columnasDelGrid)
        {
            var listaDeEstudiantes = new List<FilaDelGrid>();
            var (Estudiantes, total) = _gestordeEstudiantes.LeerTodos();
            foreach (var Estudiante in Estudiantes)
            {
                var datosDelEstudiante = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in columnasDelGrid)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(ElementoEstudiante.Id))
                        celda.Valor = Estudiante.Id.ToString();
                    else
                    if (columna.Nombre == nameof(ElementoEstudiante.Apellido))
                        celda.Valor = Estudiante.Apellido;
                    else
                    if (columna.Nombre == nameof(ElementoEstudiante.Nombre))
                        celda.Valor = Estudiante.Nombre;

                    datosDelEstudiante.Celdas.Add(celda);
                }
                listaDeEstudiantes.Add(datosDelEstudiante);
            }

            return (listaDeEstudiantes, total);
        }

        private static List<ColumnaDelGrid> DefinirColumnasGrid()
        {
            var columnasDelGrid = new List<ColumnaDelGrid>();
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoEstudiante.Id), Visible = false, Tipo = typeof(int) });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoEstudiante.Apellido) });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoEstudiante.Nombre)});
            return columnasDelGrid;
        }
    }
}
