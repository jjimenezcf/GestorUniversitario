using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;

namespace ServicioDeDatos.Callejero
{
    [Table("PROVINCIA_CP", Schema = "CALLEJERO")]
    public class ProvinciaCpDtm : RegistroDeRelacion
    {
        public int IdProvincia { get; set; }
        public ProvinciaDtm Provincia { get; set; }
        public int IdCp { get; set; }
        public CodigoPostalDtm CodigoPostal { get; set; }

        public ProvinciaCpDtm()
        {
            NombreDeLaPropiedadDelIdElemento1 = nameof(IdProvincia);
            NombreDeLaPropiedadDelIdElemento2 = nameof(IdCp);
        }
    }


    public static partial class ModeloDeCallejero
    {
        public static void ProvinciaCp(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProvinciaCpDtm>().Property(v => v.IdProvincia)
                .HasColumnName("ID_PPROVINCIA")
                .HasColumnType("INT")
                .IsRequired(true);

            modelBuilder.Entity<ProvinciaCpDtm>()
                        .HasIndex(p => p.IdProvincia)
                        .HasDatabaseName($"I_PROVINCIA_CP_ID_PROVINCIA");

            modelBuilder.Entity<ProvinciaCpDtm>()
            .HasOne(p => p.Provincia)
            .WithMany()
            .HasForeignKey(p => p.IdProvincia)
            .HasConstraintName($"FK_PROVINCIA_CP_ID_PROVINCIA")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProvinciaCpDtm>().Property(v => v.IdCp)
            .HasColumnName("ID_CP")
            .HasColumnType("INT")
            .IsRequired(true);

            modelBuilder.Entity<ProvinciaCpDtm>()
                        .HasIndex(p => p.IdCp)
                        .HasDatabaseName($"I_PROVINCIA_CP_ID_CP");

            modelBuilder.Entity<ProvinciaCpDtm>()
            .HasOne(p => p.CodigoPostal)
            .WithMany()
            .HasForeignKey(p => p.IdCp)
            .HasConstraintName($"FK_PROVINCIA_CP_ID_CP")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProvinciaCpDtm>().HasAlternateKey(p => p.IdCp).HasName("AK_PROVINCIA_CP_ID_CP");
        }
    }

}

