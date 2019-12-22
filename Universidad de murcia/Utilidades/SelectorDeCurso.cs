using AutoMapper;
using Extensiones;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ContextosDeBd;
using System.Collections.Generic;

namespace UniversidadDeMurcia.Utilidades
{
    public class SelectorDeCurso
    {
        public SelectorModal Selector { get; }

        private GestorDeCursos _gestordeCursos;

        public SelectorDeCurso(ContextoUniversitario contexto, IMapper mapeador)
        {
            _gestordeCursos =  new GestorDeCursos(contexto, mapeador);
            Selector = new SelectorModal("Curso", RenderizarTabla);
        }


        public string RenderizarTabla()
        {
            var cursos = _gestordeCursos.LeerTodos();
            var listaDeCursos = new List<FilaDelGrid>();
            foreach (var curso in cursos)
            {
                var datosDelCurso = new FilaDelGrid();
                datosDelCurso.Valores.Add(curso.Titulo);
                datosDelCurso.Valores.Add(curso.Creditos.ToString());
                listaDeCursos.Add(datosDelCurso);
            }

            var columnasDelGrid = new List<ColumnaDelGrid>(); 
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = "Título", Ordenar = false });
            columnasDelGrid.Add(new ColumnaDelGrid() { Nombre = "Créditos", Ordenar = false });

            Selector.NumeroDeColumnaDeSeleccion = 0;
            Selector.UltimaColumna = columnasDelGrid.Count;

            return HtmlRender.RenderizarTabla(Selector.Id, columnasDelGrid, listaDeCursos, true);
        }

    }
}
