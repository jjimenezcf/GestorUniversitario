using Gestor.Elementos.Seguridad;

namespace ModeloDeDto.Seguridad
{

    public class RolPermisoDto : ElementoDto
    {
        public int IdCurso { get; set; }
        public int IdEstudiante { get; set; }
        public Grado? Grado { get; set; }

        public PermisoDto Curso { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
