using Gestor.Elementos.ModeloBd;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    [Table("EST_CURSO", Schema = "UNIVERSIDAD")]
    public class RegistroDeInscripcion : Registro
    {
        public int CursoId { get; set; }
        public int EstudianteId { get; set; }
        public Grado? Grado { get; set; }

        public PermisoReg Curso { get; set; }

    }


}
