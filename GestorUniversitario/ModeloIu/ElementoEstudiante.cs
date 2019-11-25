using GestorDeElementos.IuModelo;
using System;
using System.Collections.Generic;

namespace GestorUniversitario.IuModelo
{
    public class ElementoEstudiante: ElementoBase
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<ElementoInscripcion> Inscripciones { get; set; }
    }
}
