using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Callejero
{
    [Table("PROVINCIA", Schema = "CALLEJERO")]
    public class ProvinciaDtm : ElementoDtm
    {
        public string Codigo { get; set; }
        public string Sigla { get; set; }
        public string Prefijo { get; set; }
        public int IdPais { get; set; }
        public PaisDtm Pais { get; set; }
    }
    public static class TablaProvincia
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelElementoDtm<ProvinciaDtm>(modelBuilder);
            modelBuilder.Entity<ProvinciaDtm>().Property(v => v.Codigo)
                .HasColumnName("CODIGO")
                .HasColumnType("VARCHAR(2)")
                .IsRequired(true);

            modelBuilder.Entity<ProvinciaDtm>().Property(v => v.Sigla)
                .HasColumnName("SIGLA")
                .HasColumnType("VARCHAR(3)")
                .IsRequired(true);

            modelBuilder.Entity<ProvinciaDtm>().Property(v => v.IdPais)
                .HasColumnName("ID_PAIS")
                .HasColumnType("INT")
                .IsRequired(true);

            modelBuilder.Entity<ProvinciaDtm>().Property(v => v.Prefijo)
                .HasColumnName("PREFIJO")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);

            modelBuilder.Entity<ProvinciaDtm>().HasAlternateKey(p => p.Codigo).HasName("AK_PROVINCIA_CODIGO");
            modelBuilder.Entity<ProvinciaDtm>().HasAlternateKey(p => p.Sigla).HasName("AK_PROVINCIA_SIGLA");

            modelBuilder.Entity<ProvinciaDtm>()
                        .HasIndex(p => p.IdPais)
                        .HasDatabaseName($"I_PROVINCIA_ID_PAIS");

            modelBuilder.Entity<ProvinciaDtm>()
            .HasOne(p => p.Pais)
            .WithMany()
            .HasForeignKey(p => p.IdPais)
            .HasConstraintName($"FK_PROVINCIA_ID_PAIS")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

/*            modelBuilder.Entity<TEntity>()
            .HasOne(p => p.UsuarioModificador)
            .WithMany()
            .HasForeignKey(p => p.IdUsuaModi)
            .HasConstraintName($"FK_{nombreDeTabla}_IDUSUMODI")
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TEntity>()
                        .HasIndex(p => p.IdUsuaCrea)
                        .HasDatabaseName($"I_{nombreDeTabla}_IDUSUCREA");

            modelBuilder.Entity<TEntity>()
                        .HasIndex(p => p.IdUsuaModi)
                        .HasDatabaseName($"I_{nombreDeTabla}_IDUSUMODI");
 * */