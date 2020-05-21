using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Seguridad
{


    [Table("USU_PUESTO", Schema = "SEGURIDAD")]
    public class PuestosDeUsuarioDtm : Registro
    {
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsua { get; set; }
        public virtual UsuarioDtm Usuario { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public virtual PuestoDtm Puesto { get; set; }
    }

    public static class TablaUsuPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PuestosDeUsuarioDtm>().Property(p => p.IdUsua).IsRequired();
            modelBuilder.Entity<PuestosDeUsuarioDtm>().Property(p => p.idPuesto).IsRequired();


            modelBuilder.Entity<PuestosDeUsuarioDtm>().Property(p => p.IdUsua).HasColumnName("IDUSUA");
            modelBuilder.Entity<PuestosDeUsuarioDtm>().Property(p => p.idPuesto).HasColumnName("IDPUESTO");


            modelBuilder.Entity<PuestosDeUsuarioDtm>()
                        .HasIndex(p =>new { p.idPuesto, p.IdUsua })
                        .HasName("I_USU_PUESTO_IDPUESTO_IDUSUA")
                        .IsUnique();

            modelBuilder.Entity<PuestosDeUsuarioDtm>()
                .HasIndex(p => p.IdUsua)
                .IsUnique(false)
                .HasName("I_USU_PUESTO_IDUSUA");

            modelBuilder.Entity<PuestosDeUsuarioDtm>()
                .HasIndex(p => p.idPuesto)
                .IsUnique(false)
                .HasName("I_USU_PUESTO_IDPUESTO");

            modelBuilder.Entity<PuestosDeUsuarioDtm>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_USU_PUESTO_IDPUESTO")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PuestosDeUsuarioDtm>()
                .HasOne(x => x.Usuario)
                .WithMany(p => p.Puestos)
                .HasForeignKey(x => x.IdUsua)
                .HasConstraintName("FK_USU_PUESTO_IDUSUA")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
