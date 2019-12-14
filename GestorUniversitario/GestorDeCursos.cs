using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Reflection;
using AutoMapper;
using Extensiones;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using System.Text;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeCursos : GestorDeElementos<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {
        public class MapeoRegistroCurso : Profile
        {
            public MapeoRegistroCurso()
            {
                CreateMap<RegistroDeCurso, ElementoCurso>();
                CreateMap<ElementoCurso,RegistroDeCurso>();
            }
        }

        private SelectorModal _selector;
        public SelectorModal Selector => _selector; 

        public GestorDeCursos(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            _selector = new SelectorModal("Curso", RenderizarElementos);
        }
               
        protected override RegistroDeCurso LeerConDetalle(int Id)
        {
            return null;
        }

        public string RenderizarElementos()
        {
            var cursos = LeerTodos();
            var listaDeCursos = new StringBuilder();
            int i = 0;
            foreach (var curso in cursos)
            {
                var valores = new List<string>();
                valores.Add(curso.Titulo);
                valores.Add(curso.Creditos.ToString());
                listaDeCursos.AppendLine(SelectorModal.AnadirFila(Selector.Id, i++, valores));
            }

            var cabecera = new List<string>(new string[] { "Título", "Créditos"});
            var htmlListaDeElementos = SelectorModal.AnadirTabla(cabecera, listaDeCursos);

            return htmlListaDeElementos;
        }
    }
}
