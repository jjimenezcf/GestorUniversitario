using AutoMapper;
using Gestor.Elementos.ModeloBd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Universitario.ModeloBd
{
    [Table("EST_ELEMENTO", Schema = "UNIVERSIDAD")]
    public class RegistroDeEstudiante : Registro
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }

}
