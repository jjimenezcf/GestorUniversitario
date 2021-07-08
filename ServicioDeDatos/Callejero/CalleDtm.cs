using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;

namespace ServicioDeDatos.Callejero
{
    [Table("CALLE", Schema = "CALLEJERO")]
    public class CalleDtm : ElementoDtm, IAuditoria
    {
        public string Codigo { get; set; }
        public int IdMunicipio { get; set; }
        public MunicipioDtm Municipio { get; set; }
        public int IdTipoVia { get; set; }
        public TipoDeViaDtm TipoDeVia { get; set; }

        //public IEnumerable<CpsDeUnCalleDtm> Cps { get; set; }

        public string Expresion { get; }

    }

    [Table("CALLE_AUDITORIA", Schema = "CALLEJERO")]
    public class AuditoriaDeUnCalleDtm : AuditoriaDtm
    {
        //public new virtual CalleDtm Elemento { get; set; }
    }
    public static partial class ModeloDeCallejero
    {
        public static void Calle(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelElementoDtm<CalleDtm>(modelBuilder);
            modelBuilder.Entity<CalleDtm>().Property(v => v.Codigo)
                .HasColumnName("CODIGO")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);

            modelBuilder.Entity<CalleDtm>().Property(v => v.IdMunicipio)
                .HasColumnName("ID_MUNICIPIO")
                .HasColumnType("INT")
                .IsRequired(true);

            modelBuilder.Entity<CalleDtm>().Property(v => v.IdTipoVia)
                .HasColumnName("ID_TIPO_DE_VIA")
                .HasColumnType("INT")
                .IsRequired(true);

            modelBuilder.Entity<CalleDtm>().HasAlternateKey(p => new {p.IdMunicipio, p.Codigo }).HasName("AK_CALLE_ID_MUNICIPIO_CODIGO");

            modelBuilder.Entity<CalleDtm>()
                        .HasIndex(p => p.IdMunicipio)
                        .HasDatabaseName($"I_CALLE_ID_MUNICIPIO");

            modelBuilder.Entity<CalleDtm>()
            .HasOne(p => p.Municipio)
            .WithMany()
            .HasForeignKey(p => p.IdMunicipio)
            .HasConstraintName($"FK_CALLE_ID_MUNICIPIO")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalleDtm>()
            .HasOne(p => p.TipoDeVia)
            .WithMany()
            .HasForeignKey(p => p.IdTipoVia)
            .HasConstraintName($"FK_CALLE_ID_TIPO_DE_VIA")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalleDtm>().Property(v => v.Expresion).HasColumnName("EXPRESION").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("CALLEJERO.CC_CALLE_EXPRESION(NOMBRE, ID_MUNICIPIO, ID_TIPO_DE_VIA)");

        }

        public static void CalleAudt(ModelBuilder modelBuilder)
        {
            Negocio.Auditoria.DefinirCamposDeAuditoriaDtm<AuditoriaDeUnCalleDtm>(modelBuilder);

            //modelBuilder.Entity<AuditoriaDeUnCalleDtm>()
            //.HasOne(p => p.Elemento)
            //.WithMany()
            //.HasForeignKey(p => p.IdElemento)
            //.HasConstraintName($"FK_CALLE_AUDITORIA_ID_ELEMENTO")
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
