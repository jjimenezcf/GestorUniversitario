using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;

namespace ServicioDeDatos.TrabajosSometidos
{
    [Table("TRABAJO", Schema = "TRABAJO")]
    public class TrabajoSometidoDtm : RegistroConNombre
    {
        [Column("ES_DLL", TypeName = "BIT")]
        public bool EsDll { get; set; }

        [Column("DLL", TypeName = "VARCHAR(250)")]
        public string Dll { get; set; }

        [Column("CLASE", TypeName = "VARCHAR(250)")]
        public string Clase { get; set; }

        [Column("METODO", TypeName = "VARCHAR(250)")]
        public string Metodo { get; set; }

        [Column("ESQUEMA", TypeName = "VARCHAR(250)")]
        public string Esquema { get; set; }

        [Column("PA", TypeName = "VARCHAR(250)")]
        public string Pa { get; set; }

        [Column("COMUNICAR_FIN", TypeName = "BIT")]
        public bool ComunicarFin { get; set; }

        [Column("COMUNICAR_ERROR", TypeName = "BIT")]
        public bool ComunicarError { get; set; }

        [Column("ID_EJECUTOR", TypeName = "INT")]
        public int? IdEjecutor { get; set; }
        public virtual UsuarioDtm Ejecutor { get; set; }

        [Column("ID_INFORMAR_A", TypeName = "INT")]
        public int? IdInformarA { get; set; }

        public virtual PuestoDtm InformarA { get; set; }
    }


    public static class TablaTrabajo
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.EsDll).IsRequired(true).HasDefaultValue(true);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Dll).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Clase).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Metodo).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Esquema).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.Pa).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.IdEjecutor).IsRequired(false);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.IdInformarA).IsRequired(false);

            //modelBuilder.Entity<TrabajoDtm>().HasAlternateKey(p => new { p.Dll, p.Clase, p.Metodo });
            //modelBuilder.Entity<TrabajoDtm>().HasAlternateKey(p => new { p.Esquema, p.Pa });

            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.ComunicarFin).IsRequired(true);
            modelBuilder.Entity<TrabajoSometidoDtm>().Property(p => p.ComunicarError).IsRequired(true);

            modelBuilder.Entity<TrabajoSometidoDtm>().HasOne(x => x.Ejecutor).WithMany().HasForeignKey(x => x.IdEjecutor).HasConstraintName("FK_TRABAJO_ID_EJECUTOR").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TrabajoSometidoDtm>().HasOne(x => x.InformarA).WithMany().HasForeignKey(x => x.IdInformarA).HasConstraintName("FK_TRABAJO_ID_INFORMAR_A").OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TrabajoSometidoDtm>().HasIndex(x => x.Nombre)
               .IsUnique(true)
               .HasDatabaseName("IX_TRABAJO_NOMBRE");

            modelBuilder.Entity<TrabajoSometidoDtm>().HasIndex(x => new { x.Dll, x.Clase, x.Metodo }).IsUnique(true).HasDatabaseName("IX_TRABAJO_METODO");
            modelBuilder.Entity<TrabajoSometidoDtm>().HasIndex(x => new { x.Esquema, x.Pa }).IsUnique(true).HasDatabaseName("IX_TRABAJO_PA");
        }
    }

}
