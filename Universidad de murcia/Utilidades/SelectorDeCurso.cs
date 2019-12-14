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
            var listaDeCursos = new List<List<string>>();
            foreach (var curso in cursos)
            {
                var datosDelCurso = new List<string>();
                datosDelCurso.Add(curso.Titulo);
                datosDelCurso.Add(curso.Creditos.ToString());
                listaDeCursos.Add(datosDelCurso);
            }

            var cabecera = new List<string>(new string[] { "Título", "Créditos" });

            return HtmlRender.RenderizarTabla(Selector.Id, cabecera, listaDeCursos, true);
        }

    }
}
