using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;

namespace Gestor.Elementos.Universitario.ModeloBd
{
    public class RegistroDeCurso : Registro
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
