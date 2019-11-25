using GestorDeElementos.IuModelo;

namespace GestorUniversitario.IuModelo
{    
    public enum Grado
    {
        A, B, C, D, F
    }

    public class ElementoInscripcion : ElementoBase
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public ElementoCurso Curso { get; set; }
        public ElementoEstudiante Estudiante { get; set; }

    }


}
