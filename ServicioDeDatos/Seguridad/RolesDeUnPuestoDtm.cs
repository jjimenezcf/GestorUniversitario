using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Seguridad
{

    [Table("ROL_PUESTO", Schema = "SEGURIDAD")]
    public class RolesDeUnPuestoDtm: RegistroDeRelacion
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public RolDtm Rol { get; set; }
        public PuestoDtm Puesto { get; set; }

        public RolesDeUnPuestoDtm()
        {
            NombreDeLaPropiedadDelIdElemento1 = nameof(idPuesto);
            NombreDeLaPropiedadDelIdElemento2 = nameof(IdRol);
        }


    }

    public static class TablaRolPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolesDeUnPuestoDtm>()
                .HasAlternateKey(p => new { p.IdRol, p.idPuesto })
                .HasName("AK_ROL_PUESTO");

            modelBuilder.Entity<RolesDeUnPuestoDtm>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Puestos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PUESTO_IDROL")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolesDeUnPuestoDtm>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_ROL_PUESTO_IDPUESTO")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
