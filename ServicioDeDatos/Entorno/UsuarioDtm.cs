using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Archivos;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Entorno
{
    [Table("USUARIO", Schema = "ENTORNO")]
    public class UsuarioDtm : RegistroConNombre
    {
        [Required]
        [Column("LOGIN", TypeName = "VARCHAR(50)")]
        public string Login { get; set; }

        [Required]
        [Column("APELLIDO", TypeName = "VARCHAR(250)")]
        public string Apellido { get; set; }
                
        [Required]
        [Column("F_ALTA", TypeName = "DATE")]
        public DateTime Alta { get; set; }

        [Required]
        [Column("PASSWORD", TypeName ="VARCHAR(250)")]
        public string password { get; set; }

        public int? IdArchivo { get; set; }

        [Column("ADMINISTRADOR", TypeName = "BIT")]
        public bool EsAdministrador { get; set; }

        public string eMail { get; set; }

        public virtual ArchivoDtm Archivo { get; set; }
        
        public virtual ICollection<PermisosDeUnUsuarioDtm> Permisos { get; private set; }

        public virtual ICollection<PuestosDeUnUsuarioDtm> Puestos { get; private set; }
    }
    public static class TablaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(50)").IsRequired();
            modelBuilder.Entity<UsuarioDtm>().Property(p => p.eMail).HasColumnName("EMAIL").HasColumnType("VARCHAR(50)").IsRequired().HasDefaultValue("pendiente@se.com");
            modelBuilder.Entity<UsuarioDtm>().Property(p => p.EsAdministrador).IsRequired(true).HasDefaultValue(false);

            modelBuilder.Entity<UsuarioDtm>()
            .HasIndex(v => new { v.Login })
            .IsUnique(true)
            .HasDatabaseName("IX_USUARIO");

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Puestos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Permisos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsuario);

            GeneradorMd.DefinirCampoArchivo<UsuarioDtm>(modelBuilder);            

        }


    }

}
