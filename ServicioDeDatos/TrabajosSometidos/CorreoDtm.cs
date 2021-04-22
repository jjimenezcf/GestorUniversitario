using System;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.TrabajosSometidos
{
    class CorreoDtm: Registro
    {
        public string emisor { get; set; }

        public string receptores { get; set; }

        public string asunto { get; set; }

        public string cuerpo { get; set; }

        public string elementos { get; set; }

        public string archivos { get; set; }

        public string idUsuario { get; set; }

        public DateTime creado { get; set; }

        public DateTime enviado { get; set; }

    }

    public static class TablaDeCorreos
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<CorreoDtm>().ToTable("CORREO", "TRABAJO");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("EMISOR").IsRequired(true).HasColumnType("VARCHAR(250)");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("RECEPTOR").IsRequired(true).HasColumnType("VARCHAR(2000)");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("ASUNTO").IsRequired(true).HasColumnType("VARCHAR(500)");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("CUERPO").IsRequired(true).HasColumnType("VARCHAR(MAX)");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("ELEMENTOS").IsRequired(true).HasColumnType("VARCHAR(2000)");
            mb.Entity<CorreoDtm>().Property(p => p.emisor).HasColumnName("ARCHIVOS").IsRequired(true).HasColumnType("VARCHAR(MAX)");
            mb.Entity<CorreoDtm>().Property(p => p.creado).HasColumnName("CREADO").IsRequired(true).HasColumnType("DATETIME2(7)");
            mb.Entity<CorreoDtm>().Property(p => p.creado).HasColumnName("ENVIADO").IsRequired(true).HasColumnType("DATETIME2(7)");
            //mb.Entity<CorreoDtm>().HasOne(x => x.TrabajoDeUsuario).WithMany().HasForeignKey(x => x.IdTrabajoDeUsuario).HasConstraintName("FK_LOG_DE_TRABAJO_ID_TRABAJO_USUARIO").OnDelete(DeleteBehavior.Restrict);
        }
    }
}
