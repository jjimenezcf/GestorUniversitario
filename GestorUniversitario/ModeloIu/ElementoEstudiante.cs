using GestorDeElementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace GestorUniversitario.ModeloIu
{
    public class ElementoEstudiante: ElementoBase
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
