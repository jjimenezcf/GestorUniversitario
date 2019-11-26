using Gestor.Elementos.ModeloBd;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Universitario.ModeloBd
{
    public class RegistroDeEstudiante: RegistroBase
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
