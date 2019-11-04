using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversidadDeMurcia.Models
{
    public class Curso: Objeto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
