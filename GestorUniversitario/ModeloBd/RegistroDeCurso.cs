using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Usuario
{
    [Table("CUR_ELEMENTO", Schema = "UNIVERSIDAD")]
    public class RegistroDeCurso : Registro
    {
        [Required]
        [Column("TITULO", Order = 1, TypeName = "VARCHAR(250)")]
        public string Titulo { get; set; }

        [Required]
        [Column("CREDITOS", Order = 1, TypeName = "INT")]
        [DefaultValue(0)]
        public int Creditos { get; set; }

        public ICollection<RegistroDeInscripcion> Inscripciones { get; set; }
    }
}
