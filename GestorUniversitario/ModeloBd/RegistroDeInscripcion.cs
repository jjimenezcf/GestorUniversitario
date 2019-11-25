using GestorDeElementos.ModeloBd;
using GestorUniversitario.ModeloBd.Enumerados;

namespace GestorUniversitario.ModeloBd
{    

    public class RegistroDeInscripcion : RegistroBase
    {
        public int CursoID { get; set; }
        public int EstudianteID { get; set; }
        public Grado? Grado { get; set; }

        public RegistroDeCurso Curso { get; set; }
        public RegistroDeEstudiante Estudiante { get; set; }

    }


}
