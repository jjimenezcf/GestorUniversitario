using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Seguridad
{

    [Table("ROL_PUESTO", Schema = "SEGURIDAD")]
    public class rRolPuesto: Registro
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public RolDtm Rol { get; set; }
        public rPuesto Puesto { get; set; }
    }

    public static class TablaRolPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rRolPuesto>()
                .HasAlternateKey(p => new { p.IdRol, p.idPuesto })
                .HasName("AK_ROL_PUESTO");

            modelBuilder.Entity<rRolPuesto>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Puestos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PUESTO_IDROL");

            modelBuilder.Entity<rRolPuesto>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_ROL_PUESTO_IDPUESTO");
        }
    }
}
