namespace GestorUniversitario.ModeloDeClases
{    
    public enum Grado
    {
        A, B, C, D, F
    }

    public class Inscripcion : Elemento
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public Curso Curso { get; set; }
        public Estudiante Estudiante { get; set; }

    }


}
