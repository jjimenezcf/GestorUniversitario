using System.Collections.Generic;

namespace GestorUniversitario.ModeloDeClases
{
    public class Curso : Elemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
