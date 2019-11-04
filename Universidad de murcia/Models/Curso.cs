﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversidadDeMurcia.Models
{
    //: Objeto
    public class Curso
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CursoID { get; set; }
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
