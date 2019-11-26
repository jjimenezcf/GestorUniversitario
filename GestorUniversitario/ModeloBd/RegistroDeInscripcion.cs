using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd.Enumerados;

namespace Gestor.Elementos.Universitario.ModeloBd
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
