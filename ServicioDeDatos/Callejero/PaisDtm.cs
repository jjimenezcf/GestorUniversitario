using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Callejero
{
    [Table("PAIS", Schema = "CALLEJERO")]
    public class PaisDtm : ElementoDtm
    {
        public string Codigo { get; set; }
    }

    public static class TablaPais
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelElementoDtm<PaisDtm>(modelBuilder);
            modelBuilder.Entity<PaisDtm>().Property(v => v.Codigo)
                .HasColumnName("CODIGO")
                .HasColumnType("VARCHAR(3)")
                .IsRequired(true);
        }
    }

}
