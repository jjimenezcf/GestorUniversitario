using GestorDeElementos.BdModelo;

namespace GestorUniversitario.BdModelo
{    
    public enum Grado
    {
        A, B, C, D, F
    }

    public class RegistroDeInscripcion : RegistroBase
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public RegistroDeCurso Curso { get; set; }
        public RegistroDeEstudiante Estudiante { get; set; }

    }


}
