using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Universitario.ModeloBd.Enumerados;

namespace Gestor.Elementos.Universitario.ModeloIu
{    

    public class ElementoInscripcionesDeUnEstudiante : ElementoBase
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public ElementoCurso Curso { get; set; }
        //public ElementoEstudiante Estudiante { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
