using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

    [Table("PUESTO", Schema = "SEGURIDAD")]
    public class rPuesto : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<rRolPuesto> Roles { get; set; }
        public ICollection<RegUsuPuesto> Usuarios { get; set; }
    }

    public static class TablaPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rPuesto>()
            .HasAlternateKey(p => p.Nombre)
            .HasName("AK_PUESTO_NOMBRE");
        }
    }
}
