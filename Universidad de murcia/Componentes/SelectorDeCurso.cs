using AutoMapper;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Collections.Generic;
using UtilidadesParaIu;

namespace Componentes
{
    public class SelectorDeCurso
    {
        public SelectorModal Selector { get; }

        private GestorDeCursos _gestordeCursos;

        public SelectorDeCurso(ContextoUniversitario contexto, IMapper mapeador)
        {
            _gestordeCursos = new GestorDeCursos(contexto, mapeador);
            Selector = new SelectorModal(nameof(ElementoCurso), RenderizarTabla)
            {
                ColumnaId = nameof(ElementoCurso.Id),
                ColumnaMostrar = nameof(ElementoCurso.Titulo)
            };
        }


        public string RenderizarTabla()
        {
            var descriptorColumnas = DefinirColumnasGrid();
            var filas = ObtenerFilasDelGrid(descriptorColumnas);

            Grid grid = new Grid(Selector.IdTabla, descriptorColumnas, filas.filas) {
                Ruta = "Cursos",
                TotalEnBd = filas.totalBd,
                ConNavegador = true,
                ConSeleccion = true
            };

            return grid.ToHtml();
        }

        private (List<FilaDelGrid> filas, int totalBd) ObtenerFilasDelGrid(List<ColumnaDelGrid> columnasDelGrid)
        {
            var listaDeCursos = new List<FilaDelGrid>();
            var (cursos,total) = _gestordeCursos.LeerTodos();
            
            foreach (var curso in cursos)
            {
                var datosDelCurso = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in columnasDelGrid)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(ElementoCurso.Id))
                        celda.Valor = curso.Id.ToString();
                    else
                    if (columna.Nombre == nameof(ElementoCurso.Titulo))
                        celda.Valor = curso.Titulo;
                    else
                    if (columna.Nombre == nameof(ElementoCurso.Creditos))
                        celda.Valor = curso.Creditos.ToString();

                    datosDelCurso.Celdas.Add(celda);
                }
                listaDeCursos.Add(datosDelCurso);
            }

            return (listaDeCursos,total);
        }

        private static List<ColumnaDelGrid> DefinirColumnasGrid()
        {
            var columnasDelGrid = new List<ColumnaDelGrid>();
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Id), Visible = false, Tipo = typeof(int) });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Titulo), Titulo = "Título", Ordenar = false });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Creditos), Titulo = "Créditos", Tipo = typeof(int) });
            return columnasDelGrid;
        }
    }
}
