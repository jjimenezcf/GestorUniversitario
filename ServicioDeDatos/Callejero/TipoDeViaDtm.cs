using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;

namespace ServicioDeDatos.Callejero
{
    [Table("TIPO_VIA", Schema = "CALLEJERO")]
    public class TipoDeViaDtm : RegistroConNombre
    {
        public string Sigla { get; set; }
    }

    public static partial class ModeloDeCallejero
    {
        public static void TipoVia(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelRegistroConNombreDtm<TipoDeViaDtm>(modelBuilder);
            
            modelBuilder.Entity<TipoDeViaDtm>().Property(v => v.Sigla)
                .HasColumnName("SIGLA")
                .HasColumnType("VARCHAR(4)")
                .IsRequired(true);

            modelBuilder.Entity<TipoDeViaDtm>().HasAlternateKey(p => p.Sigla).HasName("AK_TIPO_VIA_SIGLA");
        }

    }

}
