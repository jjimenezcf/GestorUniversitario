using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("VISTA_MVC", Schema = "ENTORNO")]
    public class R_VistaMvc : Registro
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

        public List<R_Menu> Menus { get; set; }
    }


    [Table("MENU", Schema = "ENTORNO")]
    public class R_Menu : Registro
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
        [Column("ACTIVO", TypeName = "BIT")]
        public bool Activo { get; set; }

        [Column("IDPADRE", TypeName = "INT")]
        public int? IdPadre { get; set; }

        public R_Menu Padre { get; set; }

        [Column("IDVISTA_MVC", TypeName = "INT")]
        public int? IdVistaMvc { get; set; }

        public virtual R_VistaMvc VistaMvc { get; set; }
    }


    public static class TablaVistaMvc
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<R_VistaMvc>().Property(p => p.Parametros).IsRequired(false);

            modelBuilder.Entity<R_VistaMvc>()
               .HasIndex(a => new { a.Controlador, a.Accion, a.Parametros })
               .IsUnique(true)
               .HasName("IX_VISTA_MVC");

            modelBuilder.Entity<R_VistaMvc>()
                .HasMany(a => a.Menus)
                .WithOne(a => a.VistaMvc)
                .HasForeignKey(f=>f.IdVistaMvc); 
        }
    }

    public static class TablaMenu
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<R_Menu>().Property(p => p.IdPadre).IsRequired(false);
            modelBuilder.Entity<R_Menu>().Property(p => p.IdVistaMvc).IsRequired(false);
            
            modelBuilder.Entity<R_Menu>()
                .HasOne(f => f.Padre)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(f => f.IdPadre)
                .HasConstraintName("FK_MENU_IDPADRE");

            modelBuilder.Entity<R_Menu>()
                        .HasOne(f => f.VistaMvc)
                        .WithMany(a => a.Menus)
                        .IsRequired(false)
                        .HasForeignKey(f => f.IdVistaMvc)
                        .HasConstraintName("FK_MENU_IDVISTA_MVC"); 
        }
    }


}
