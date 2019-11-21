using GestorDeElementos.BdModelo;

namespace GestorUniversitario.BdModelo
{    
    public enum Grado
    {
        A, B, C, D, F
    }

    public class BdInscripcion : BdElemento
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public BdCurso Curso { get; set; }
        public BdEstudiante Estudiante { get; set; }

    }


}
