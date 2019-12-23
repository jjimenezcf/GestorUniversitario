using AutoMapper;
using Extensiones;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Collections.Generic;

namespace UniversidadDeMurcia.Utilidades
{
    public class SelectorDeCurso
    {
        public SelectorModal Selector { get; }

        private GestorDeCursos _gestordeCursos;

        public SelectorDeCurso(ContextoUniversitario contexto, IMapper mapeador)
        {
            _gestordeCursos = new GestorDeCursos(contexto, mapeador);
            Selector = new SelectorModal("Curso", RenderizarTabla);
        }


        public string RenderizarTabla()
        {
            var cursos = _gestordeCursos.LeerTodos();

            var columnasDelGrid = new List<ColumnaDelGrid>();
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = "Id", Visible = false, Tipo = typeof(int) });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = "Título", Ordenar = false });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = "Créditos", Tipo = typeof(int) });

            var listaDeCursos = new List<FilaDelGrid>();
            foreach (var curso in cursos)
            {
                var datosDelCurso = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in columnasDelGrid)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == "Id")
                        celda.Valor = curso.Id.ToString();
                    else
                    if (columna.Nombre == "Título")
                        celda.Valor = curso.Titulo;
                    else
                    if (columna.Nombre == "Créditos")
                        celda.Valor = curso.Creditos.ToString();
                    datosDelCurso.Celdas.Add(celda);
                }
                listaDeCursos.Add(datosDelCurso);
            }

            Selector.NumeroDeColumnaDeSeleccion = 0;
            Selector.UltimaColumna = columnasDelGrid.Count;

            return HtmlRender.RenderizarTabla(Selector.Id, columnasDelGrid, listaDeCursos, true);
        }

    }
}
