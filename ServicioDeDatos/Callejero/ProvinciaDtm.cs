using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;

namespace ServicioDeDatos.Callejero
{
    [Table("PROVINCIA", Schema = "CALLEJERO")]
    public class ProvinciaDtm : ElementoDtm, IAuditoria
    {
        public string Codigo { get; set; }
        public string Sigla { get; set; }
        public string Prefijo { get; set; }
        public int IdPais { get; set; }
        public PaisDtm Pais { get; set; }
    }

    [Table("PROVINCIA_AUDITORIA", Schema = "CALLEJERO")]
    public class AuditoriaDeUnaProvinciaDtm: AuditoriaDtm
    {
        //public new virtual ProvinciaDtm Elemento { get; set; }
    }

    public static partial class ModeloDeCallejero
    {
        public static void Provincia(ModelBuilder modelBuilder)
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

        public static void ProvinciaAudt(ModelBuilder modelBuilder)
        {
            Negocio.Auditoria.DefinirCamposDeAuditoriaDtm<AuditoriaDeUnaProvinciaDtm>(modelBuilder);

            //modelBuilder.Entity<AuditoriaDeUnaProvinciaDtm>()
            //.HasOne(p => p.Elemento)
            //.WithMany()
            //.HasForeignKey(p => p.IdElemento)
            //.HasConstraintName($"FK_PROVINCIA_AUDITORIA_ID_ELEMENTO")
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }

}

