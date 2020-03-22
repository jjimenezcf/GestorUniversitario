using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Entorno
{
    [Table("USUARIO", Schema = "ENTORNO")]
    public class rUsuario : Registro
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

    }
    public static class TablaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rUsuario>()
            .HasIndex(v => new { v.Login })
            .IsUnique(true)
            .HasName("IX_USUARIO");

        }
    }

}
