using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;



namespace ServicioDeDatos.TrabajosSometidos
{

    [Table("SEMAFORO", Schema = "TRABAJO")]
    public class SemaforoDeTrabajosDtm : Registro
    {
        public int IdTrabajoUsuario { get; set; }
        public DateTime Iniciado { get; set; }
        public string login { get; set; }
    }

    public static class TablaSemaforoDeTrabajos
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<SemaforoDeTrabajosDtm>().Property(p => p.IdTrabajoUsuario).HasColumnName("ID_TRABAJO").IsRequired(true).HasColumnType("INT");
            mb.Entity<SemaforoDeTrabajosDtm>().Property(p => p.Iniciado).HasColumnName("INICIADO").IsRequired(true).HasColumnType("DATETIME");
            mb.Entity<SemaforoDeTrabajosDtm>().Property(p => p.login).HasColumnName("LOGIN").IsRequired(true).HasColumnType("VARCHAR(50)");
            mb.Entity<SemaforoDeTrabajosDtm>().HasAlternateKey(p => p.IdTrabajoUsuario).HasName($"AK_SEMAFORO_TRABAJO_ID_TRABAJO");
        }
    }
}
