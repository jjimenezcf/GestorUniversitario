using GestorDeElementos.BdModelo;
using System.Collections.Generic;

namespace GestorUniversitario.BdModelo
{
    public class RegistroDeCurso : RegistroBase
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
