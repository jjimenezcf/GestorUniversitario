using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("ACCION", Schema = "ENTORNO")]
    public class RegAccion : Registro
    {
        [Required]
        [Column("NOMBRE",  TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }
        [Required]
        [Column("CONTROLADOR",  TypeName = "VARCHAR(250)")]
        public string Controlador { get; set; }

        [Required]
        [Column("ACCION",  TypeName = "VARCHAR(250)")]
        public string Accion { get; set; }

        [Column("PARAMETROS", TypeName = "VARCHAR(250)")]
        public string Parametros { get; set; }

        public List<RegFuncion> Funciones { get; set; }
    }


    [Table("FUNCION", Schema = "ENTORNO")]
    public class RegFuncion : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("ICONO", TypeName = "VARCHAR(250)")]
        public string ICONO { get; set; }

        [Required]
        [Column("ACTIVO", Order = 5, TypeName = "CHAR(1)")]
        public char Activo { get; set; }

        [Column("IDPADRE", TypeName = "INT")]
        public int? IdPadre { get; set; }

        public RegFuncion Padre { get; set; }

        [Column("IDACCION", TypeName = "INT")]
        public int? IdAccion { get; set; }

        public virtual RegAccion Accion { get; set; }
    }


    public static class TablaAccion
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegAccion>().Property(p => p.Parametros).IsRequired(false);

            modelBuilder.Entity<RegAccion>()
               .HasIndex(a => new { a.Controlador, a.Accion, a.Parametros })
               .IsUnique(true)
               .HasName("IX_ACCION");

            modelBuilder.Entity<RegAccion>()
                .HasMany(a => a.Funciones)
                .WithOne(a => a.Accion)
                .HasForeignKey(f=>f.IdAccion); 
        }
    }

    public static class TablaFuncion
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegFuncion>().Property(p => p.IdPadre).IsRequired(false);
            modelBuilder.Entity<RegFuncion>().Property(p => p.IdAccion).IsRequired(false);
            
            modelBuilder.Entity<RegFuncion>()
                .HasOne(f => f.Padre)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(f => f.IdPadre)
                .HasConstraintName("FK_FUNCION_IDPADRE");

            modelBuilder.Entity<RegFuncion>()
                        .HasOne(f => f.Accion)
                        .WithMany(a => a.Funciones)
                        .IsRequired(false)
                        .HasForeignKey(f => f.IdAccion)
                        .HasConstraintName("FK_FUNCION_IDACCION"); 
        }
    }


}
