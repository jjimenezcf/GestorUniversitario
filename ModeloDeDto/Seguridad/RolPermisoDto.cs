
using ServicioDeDatos.Seguridad;

namespace ModeloDeDto.Seguridad
{

    public class RolPermisoDto : ElementoDto
    {
        public int IdCurso { get; set; }
        public int IdEstudiante { get; set; }
        public enumGrado? Grado { get; set; }

        public PermisoDto Curso { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
