using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Usuario
{

    public class ElementoInscripcionesDeUnEstudiante : Elemento
    {
        public int IdCurso { get; set; }
        public int IdEstudiante { get; set; }
        public Grado? Grado { get; set; }

        public ElementoCurso Curso { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
