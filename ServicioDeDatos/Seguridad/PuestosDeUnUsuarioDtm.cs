﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Seguridad
{


    [Table("USU_PUESTO", Schema = "SEGURIDAD")]
    public class PuestosDeUnUsuarioDtm : RegistroDeRelacion
    {
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsuario { get; set; }
        public virtual UsuarioDtm Usuario { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int IdPuesto { get; set; }

        public string RolesDeUnPuesto { get; }

        public virtual PuestoDtm Puesto { get; set; }

        public PuestosDeUnUsuarioDtm()
        {
            NombreDeLaPropiedadDelIdElemento1 = nameof(IdUsuario);
            NombreDeLaPropiedadDelIdElemento2 = nameof(IdPuesto);
        }
    }

    public static class TablaUsuPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>().Property(p => p.IdUsuario).IsRequired();
            modelBuilder.Entity<PuestosDeUnUsuarioDtm>().Property(p => p.IdPuesto).IsRequired();


            modelBuilder.Entity<PuestosDeUnUsuarioDtm>().Property(p => p.IdUsuario).HasColumnName("IDUSUA");
            modelBuilder.Entity<PuestosDeUnUsuarioDtm>().Property(p => p.IdPuesto).HasColumnName("IDPUESTO");

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>().Property(p => p.RolesDeUnPuesto).HasColumnName("ROLES").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("SEGURIDAD.OBTENER_ROLES_DE_UN_PUESTO(idPuesto)");

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>()
                        .HasIndex(p => new { p.IdPuesto, p.IdUsuario })
                        .HasDatabaseName("I_USU_PUESTO_IDPUESTO_IDUSUA")
                        .IsUnique();

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>()
                .HasIndex(p => p.IdUsuario)
                .IsUnique(false)
                .HasDatabaseName("I_USU_PUESTO_IDUSUA");

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>()
                .HasIndex(p => p.IdPuesto)
                .IsUnique(false)
                .HasDatabaseName("I_USU_PUESTO_IDPUESTO");

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.IdPuesto)
                .HasConstraintName("FK_USU_PUESTO_IDPUESTO")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PuestosDeUnUsuarioDtm>()
                .HasOne(x => x.Usuario)
                .WithMany(p => p.Puestos)
                .HasForeignKey(x => x.IdUsuario)
                .HasConstraintName("FK_USU_PUESTO_IDUSUA")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
