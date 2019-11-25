using GestorDeElementos.IuModelo;
using System.Collections.Generic;

namespace GestorUniversitario.IuModelo
{
    public class ElementoCurso : ElementoBase
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<ElementoInscripcion> Inscripciones { get; set; }
    }
}
