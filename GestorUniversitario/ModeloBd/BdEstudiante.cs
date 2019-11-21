using GestorDeElementos.BdModelo;
using System;
using System.Collections.Generic;

namespace GestorUniversitario.BdModelo
{
    public class BdEstudiante: BdElemento
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<BdInscripcion> Inscripciones { get; set; }
    }
}
