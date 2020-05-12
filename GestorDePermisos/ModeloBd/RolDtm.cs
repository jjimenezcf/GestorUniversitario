using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{
   
    [Table("ROL", Schema = "SEGURIDAD")]
    public class RolDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RolPermisoDtm> Permisos { get; set; }
        public ICollection<rRolPuesto> Puestos { get; set; }
    }

    public static class TablaRol
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolDtm>()
                        .HasAlternateKey(p => p.Nombre)
                        .HasName("AK_ROL_NOMBRE");
        }
    }
}
