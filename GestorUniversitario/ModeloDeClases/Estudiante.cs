using System;
using System.Collections.Generic;

namespace GestorUniversitario.ModeloDeClases
{
    public class Estudiante: Elemento
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
