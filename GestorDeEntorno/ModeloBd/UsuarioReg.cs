using Gestor.Elementos.ModeloBd;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Entorno
{
    [Table("USUARIO", Schema = "ENTORNO")]
    public class UsuarioReg : Registro
    {
        [Required]
        [Column("LOGIN", Order = 1, TypeName = "VARCHAR(50)")]
        public string Login { get; set; }

        [Required]
        [Column("APELLIDO", Order = 2, TypeName = "VARCHAR(250)")]
        public string Apellido { get; set; }


        [Required]
        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }


        [Required]
        [Column("F_ALTA", Order = 4, TypeName = "DATE")]
        public DateTime Alta { get; set; }

    }

}
