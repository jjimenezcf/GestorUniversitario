using System.Collections.Generic;
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

        public string NombreProvincia { get; }
        public string NombreMunicipio { get; }
        public List<CpsDeUnaProvinciaDtm> Provincias { get; set; }
        public List<CpsDeUnMunicipioDtm> Municipios { get; set; }
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

            modelBuilder.Entity<CodigoPostalDtm>().Property(v => v.NombreProvincia).HasColumnName("PROVINCIA").HasColumnType("VARCHAR(250)").HasComputedColumnSql("CALLEJERO.CC_CODIGO_POSTAL_PROVINCIA(CP)");
            modelBuilder.Entity<CodigoPostalDtm>().Property(v => v.NombreMunicipio).HasColumnName("MUNICIPIOS").HasColumnType("VARCHAR(250)").HasComputedColumnSql("CALLEJERO.CC_CODIGO_POSTAL_MUNICIPIOS(CP)");

            modelBuilder.Entity<CodigoPostalDtm>()
                    .HasMany(p => p.Provincias)
                    .WithOne(p => p.CodigoPostal)
                    .HasForeignKey(p => p.IdProvincia);
        }
    }

}
