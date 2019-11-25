using GestorDeElementos.ModeloBd;
using System.Collections.Generic;

namespace GestorUniversitario.ModeloBd
{
    public class RegistroDeCurso : RegistroBase
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
