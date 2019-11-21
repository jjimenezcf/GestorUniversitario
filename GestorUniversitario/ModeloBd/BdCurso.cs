using GestorDeElementos.BdModelo;
using System.Collections.Generic;

namespace GestorUniversitario.BdModelo
{
    public class BdCurso : BdElemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<BdInscripcion> Inscripciones { get; set; }
    }
}
