using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("VISTA_MVC", Schema = "ENTORNO")]
    public class rVistaMvc : Registro
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

        public List<rMenu> Menus { get; set; }
    }


    [Table("MENU", Schema = "ENTORNO")]
    public class rMenu : Registro
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

        public rMenu Padre { get; set; }

        [Column("IDVISTA_MVC", TypeName = "INT")]
        public int? IdVistaMvc { get; set; }

        public virtual rVistaMvc VistaMvc { get; set; }
    }


    public static class TablaVistaMvc
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rVistaMvc>().Property(p => p.Parametros).IsRequired(false);

            modelBuilder.Entity<rVistaMvc>()
               .HasIndex(a => new { a.Controlador, a.Accion, a.Parametros })
               .IsUnique(true)
               .HasName("IX_VISTA_MVC");

            modelBuilder.Entity<rVistaMvc>()
                .HasMany(a => a.Menus)
                .WithOne(a => a.VistaMvc)
                .HasForeignKey(f=>f.IdVistaMvc); 
        }
    }

    public static class TablaMenu
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rMenu>().Property(p => p.IdPadre).IsRequired(false);
            modelBuilder.Entity<rMenu>().Property(p => p.IdVistaMvc).IsRequired(false);
            
            modelBuilder.Entity<rMenu>()
                .HasOne(f => f.Padre)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(f => f.IdPadre)
                .HasConstraintName("FK_MENU_IDPADRE");

            modelBuilder.Entity<rMenu>()
                        .HasOne(f => f.VistaMvc)
                        .WithMany(a => a.Menus)
                        .IsRequired(false)
                        .HasForeignKey(f => f.IdVistaMvc)
                        .HasConstraintName("FK_MENU_IDVISTA_MVC"); 
        }
    }


}
