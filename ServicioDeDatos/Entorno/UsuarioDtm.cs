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
    public class UsuarioDtm : Registro
    {
        [Required]
        [Column("LOGIN", Order = 1, TypeName = "VARCHAR(50)")]
        public string Login { get; set; }

        [Required]
        [Column("APELLIDO", Order = 2, TypeName = "VARCHAR(250)")]
        public string Apellido { get; set; }


        [Required]
        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }
        
        [Required]
        [Column("F_ALTA", Order = 4, TypeName = "DATE")]
        public DateTime Alta { get; set; }


        public virtual ICollection<UsuariosDeUnPermisoDtm> Permisos { get; private set; }

        public virtual ICollection<PuestosDeUsuarioDtm> Puestos { get; private set; }

    }
    public static class TablaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioDtm>()
            .HasIndex(v => new { v.Login })
            .IsUnique(true)
            .HasName("IX_USUARIO");

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Puestos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsua);

            modelBuilder.Entity<UsuarioDtm>()
                    .HasMany(tu => tu.Permisos)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey(p => p.IdUsua);

        }


    }

}
