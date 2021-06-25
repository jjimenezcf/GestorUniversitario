using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Callejero
{
    [Table("CODIGO_POSTAL", Schema = "CALLEJERO")]
    public class CodigoPostalDtm : Registro
    {
        public string Codigo { get; set; }

        [Ignore]
        public CpsDeUnaProvinciaDtm cpsProvincias { get; set; }
    }

    public static partial class ModeloDeCallejero
    {
        public static void CodigoPostal(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<CodigoPostalDtm>().Property(v => v.Codigo)
                .HasColumnName("CP")
                .HasColumnType("VARCHAR(5)")
                .IsRequired(true);

            modelBuilder.Entity<CodigoPostalDtm>().HasAlternateKey(p => p.Codigo).HasName("AK_CODIGO_POSTAL_CP");
        }
    }

}
