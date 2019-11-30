using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Universitario.ModeloBd.Enumerados;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Universitario.ModeloIu
{    

    public class ElementoInscripcionesDeUnEstudiante : ElementoBase
    {
        public int IdCurso { get; set; }
        public int IdEstudiante { get; set; }
        public Grado? Grado { get; set; }

        public ElementoCurso Curso { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
