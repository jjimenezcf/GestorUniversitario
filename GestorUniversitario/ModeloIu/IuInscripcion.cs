using GestorDeElementos.IuModelo;

namespace GestorUniversitario.IuModelo
{    
    public enum Grado
    {
        A, B, C, D, F
    }

    public class IuInscripcion : IuElemento
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public IuCurso Curso { get; set; }
        public IuEstudiante Estudiante { get; set; }

    }


}
