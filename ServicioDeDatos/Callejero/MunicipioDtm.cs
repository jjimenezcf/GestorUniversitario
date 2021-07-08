using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Negocio;

namespace ServicioDeDatos.Callejero
{
    [Table("MUNICIPIO", Schema = "CALLEJERO")]
    public class MunicipioDtm : ElementoDtm, IAuditoria
    {
        public string Codigo { get; set; }
        public string DC { get; set; }
        public int IdProvincia { get; set; }
        public ProvinciaDtm Provincia { get; set; }
        public IEnumerable<CpsDeUnMunicipioDtm> Cps { get; set; }

        public string Expresion => $"({Codigo}) {Nombre}";

    }

    [Table("MUNICIPIO_AUDITORIA", Schema = "CALLEJERO")]
    public class AuditoriaDeUnMunicipioDtm : AuditoriaDtm
    {
        //public new virtual MunicipioDtm Elemento { get; set; }
    }
    public static partial class ModeloDeCallejero
    {
        public static void Municipio(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelElementoDtm<MunicipioDtm>(modelBuilder);
            modelBuilder.Entity<MunicipioDtm>().Property(v => v.Codigo)
                .HasColumnName("CODIGO")
                .HasColumnType("VARCHAR(3)")
                .IsRequired(true);

            modelBuilder.Entity<MunicipioDtm>().Property(v => v.DC)
                .HasColumnName("DC")
                .HasColumnType("VARCHAR(1)")
                .IsRequired(true);

            modelBuilder.Entity<MunicipioDtm>().Property(v => v.IdProvincia)
                .HasColumnName("ID_PROVINCIA")
                .HasColumnType("INT")
                .IsRequired(true);

            modelBuilder.Entity<MunicipioDtm>().HasAlternateKey(p => new {p.IdProvincia, p.Codigo }).HasName("AK_MUNICIPIO_ID_PROVINCIA_CODIGO");

            modelBuilder.Entity<MunicipioDtm>()
                        .HasIndex(p => p.IdProvincia)
                        .HasDatabaseName($"I_MUNICIPIO_ID_PROVINCIA");

            modelBuilder.Entity<MunicipioDtm>()
            .HasOne(p => p.Provincia)
            .WithMany()
            .HasForeignKey(p => p.IdProvincia)
            .HasConstraintName($"FK_MUNICIPIO_ID_PROVINCIA")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MunicipioDtm>().Ignore(x => x.Expresion);
        }

        public static void MunicipioAudt(ModelBuilder modelBuilder)
        {
            Negocio.Auditoria.DefinirCamposDeAuditoriaDtm<AuditoriaDeUnMunicipioDtm>(modelBuilder);

            //modelBuilder.Entity<AuditoriaDeUnMunicipioDtm>()
            //.HasOne(p => p.Elemento)
            //.WithMany()
            //.HasForeignKey(p => p.IdElemento)
            //.HasConstraintName($"FK_MUNICIPIO_AUDITORIA_ID_ELEMENTO")
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
