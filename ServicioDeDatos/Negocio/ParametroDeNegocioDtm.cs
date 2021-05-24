using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;

namespace ServicioDeDatos.Negocio
{
    [Table("PARAMETRO", Schema = "NEGOCIO")]
    public class ParametroDeNegocioDtm : RegistroConNombre
    {
        public string Valor { get; set; }

        [Column("IDNEGOCIO", TypeName = "INT")]
        public int IdNegocio { get; set; }
        public virtual NegocioDtm Negocio { get; set; }

    }


    public static class TablaParametro
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParametroDeNegocioDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();

            modelBuilder.Entity<ParametroDeNegocioDtm>().Property(p => p.Valor).HasColumnName("VALOR").HasColumnType("VARCHAR(250)").IsRequired(true);

            modelBuilder.Entity<ParametroDeNegocioDtm>().Property(p => p.IdNegocio).HasColumnName("ID_NEGOCIO").HasColumnType("INT").IsRequired(true);

            modelBuilder.Entity<ParametroDeNegocioDtm>()
            .HasOne(p => p.Negocio)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdNegocio)
            .HasConstraintName("FK_NEGOCIO_PARAMETRO_ID_NEGOCIO")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParametroDeNegocioDtm>()
               .HasIndex(p => new { p.IdNegocio, p.Nombre})
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_ID_NEGOCIO_NOMBRE");

        }
    }

}
