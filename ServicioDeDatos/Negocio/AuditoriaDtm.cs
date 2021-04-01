using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Negocio
{
    public interface IAuditoria
    {

    }

    public class AuditoriaDtm : Registro
    {
        public int IdElemento { get; set; }
        
        public int IdUsuario { get; set; }

        public string Operacion { get; set; }

        public string registroJson { get; set; }

        public DateTime AuditadoEl { get; set; }
    }

    public static class Auditoria
    {
        internal static void DefinirCamposDeAuditoriaDtm<TEntity>(ModelBuilder modelBuilder) where TEntity : AuditoriaDtm
        {
            var nombreDeTabla = GeneradorMd.NombreDeTabla(typeof(TEntity));

            modelBuilder.Entity<TEntity>().Property(p => p.IdElemento).HasColumnName("ID_ELEMENTO");
            modelBuilder.Entity<TEntity>().Property(p => p.IdElemento).HasColumnType("INT");
            modelBuilder.Entity<TEntity>().Property(p => p.IdElemento).IsRequired(true);
            modelBuilder.Entity<TEntity>().HasIndex(p => p.IdElemento).HasDatabaseName($"I_{nombreDeTabla}_ID_ELEMENTO");

            modelBuilder.Entity<TEntity>().Property(p => p.IdUsuario).HasColumnName("ID_USUARIO");
            modelBuilder.Entity<TEntity>().Property(p => p.IdUsuario).HasColumnType("INT");
            modelBuilder.Entity<TEntity>().Property(p => p.IdUsuario).IsRequired(true);
            modelBuilder.Entity<TEntity>().HasIndex(p => p.IdUsuario).HasDatabaseName($"I_{nombreDeTabla}_ID_USUARIO");

            modelBuilder.Entity<TEntity>().Property(p => p.Operacion).HasColumnName("OPERACION");
            modelBuilder.Entity<TEntity>().Property(p => p.Operacion).HasColumnType("CHAR(1)");
            modelBuilder.Entity<TEntity>().Property(p => p.Operacion).IsRequired(true);

            modelBuilder.Entity<TEntity>().Property(p => p.registroJson).HasColumnName("REGISTRO");
            modelBuilder.Entity<TEntity>().Property(p => p.registroJson).HasColumnType("VARCHAR(MAX)");
            modelBuilder.Entity<TEntity>().Property(p => p.registroJson).IsRequired(true);

            modelBuilder.Entity<TEntity>().Property(p => p.AuditadoEl).HasColumnName("AUDITADO_EL");
            modelBuilder.Entity<TEntity>().Property(p => p.AuditadoEl).HasColumnType("DATETIME");
            modelBuilder.Entity<TEntity>().Property(p => p.AuditadoEl).IsRequired(true);

        }
        public static bool ImplementaAuditoria(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(IAuditoria));
        }
        public static bool ImplementaAuditoria(this Registro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(IAuditoria));
        }
    }

}
