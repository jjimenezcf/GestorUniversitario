using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class UsuarioDtm : Registro
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

        public int? IdArchivo { get; set; }

        public virtual ArchivoDtm Archivo { get; set; }
        
        public virtual ICollection<UsuariosDeUnPermisoDtm> Permisos { get; private set; }

        public virtual ICollection<PuestosDeUnUsuarioDtm> Puestos { get; private set; }

    }
    public static class TablaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(50)").IsRequired();

            modelBuilder.Entity<UsuarioDtm>()
            .HasIndex(v => new { v.Login })
            .IsUnique(true)
            .HasName("IX_USUARIO");

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Puestos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Permisos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsua);

            GeneradorMd.DefinirCampoArchivo<UsuarioDtm>(modelBuilder);            

        }


    }

}
