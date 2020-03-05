using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Usuario.ModeloBd.Enumerados;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Usuario.ModeloIu
{    

    public class ElementoInscripcionesDeUnEstudiante : Elemento
    {
        public int IdCurso { get; set; }
        public int IdEstudiante { get; set; }
        public Grado? Grado { get; set; }

        public ElementoCurso Curso { get; set; }

        public string PropiedadCurso => nameof(Curso);
    }


}
