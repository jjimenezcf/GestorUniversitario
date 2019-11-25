using GestorDeElementos.ModeloBd;
using System;
using System.Collections.Generic;

namespace GestorUniversitario.ModeloBd
{
    public class RegistroDeEstudiante: RegistroBase
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
