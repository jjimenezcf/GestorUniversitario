using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd.Enumerados;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Universitario.ModeloBd
{    

    public class RegistroDeInscripcion : RegistroBase
    {
        public int CursoId { get; set; }
        public int EstudianteId { get; set; }
        public Grado? Grado { get; set; }

        public RegistroDeCurso Curso { get; set; }
        public RegistroDeEstudiante Estudiante { get; set; }

    }


}
