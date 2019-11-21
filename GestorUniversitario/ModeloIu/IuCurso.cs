using GestorDeElementos.IuModelo;
using System.Collections.Generic;

namespace GestorUniversitario.IuModelo
{
    public class IuCurso : IuElemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<IuInscripcion> Inscripciones { get; set; }
    }
}
