using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversidadDeMurcia.Models
{
    public class Curso : Elemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
