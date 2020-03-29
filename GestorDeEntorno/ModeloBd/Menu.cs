using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("VISTA_MVC", Schema = "ENTORNO")]
    public class VistaDtm : Registro
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

        public List<MenuDtm> Menus { get; set; }
    }


    [Table("MENU", Schema = "ENTORNO")]
    public class MenuDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("ICONO", TypeName = "VARCHAR(250)")]
        public string Icono { get; set; }

        [Required]
        [Column("ORDEN", TypeName = "INT")]
        public int orden { get; set; }

        [Required]
        [Column("ACTIVO", TypeName = "BIT")]
        public bool Activo { get; set; }

        [Column("IDPADRE", TypeName = "INT")]
        public int? IdPadre { get; set; }

        public MenuDtm Padre { get; set; }

        public List<MenuDtm> Submenus { get; set; }

        [Column("IDVISTA_MVC", TypeName = "INT")]
        public int? IdVistaMvc { get; set; }

        public virtual VistaDtm VistaMvc { get; set; }

    }


    public static class TablaVistaMvc
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VistaDtm>().Property(menu => menu.Parametros).IsRequired(false);

            modelBuilder.Entity<VistaDtm>()
               .HasIndex(vista => new { vista.Controlador, vista.Accion, vista.Parametros })
               .IsUnique(true)
               .HasName("IX_VISTA_MVC");

            modelBuilder.Entity<VistaDtm>()
                .HasMany(vista => vista.Menus)
                .WithOne(vista => vista.VistaMvc)
                .HasForeignKey(menu=>menu.IdVistaMvc); 
        }
    }

    public static class TablaMenu
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuDtm>().Property(menu => menu.IdPadre).IsRequired(false);
            modelBuilder.Entity<MenuDtm>().Property(menu => menu.IdVistaMvc).IsRequired(false);

            modelBuilder.Entity<MenuDtm>().Property(menu => menu.orden).HasDefaultValue(0);

            modelBuilder.Entity<MenuDtm>()
                .HasOne(menu => menu.Padre)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(menu => menu.IdPadre)
                .HasConstraintName("FK_MENU_IDPADRE");

            modelBuilder.Entity<MenuDtm>()
                        .HasOne(menu => menu.VistaMvc)
                        .WithMany(vista => vista.Menus)
                        .IsRequired(false)
                        .HasForeignKey(menu => menu.IdVistaMvc)
                        .HasConstraintName("FK_MENU_IDVISTA_MVC");

            modelBuilder.Entity<MenuDtm>()
                .HasMany(menu => menu.Submenus)
                .WithOne(m => m.Padre)
                .IsRequired(false);

            //modelBuilder.Entity<R_Menu>()
            //            .HasMany(menu => menu.Submenus)
            //            .WithOne()
            //            //.HasForeignKey("IDPADRE")
            //            //.HasConstraintName("FK_MENU_IDPADRE")
            //            .IsRequired(false);
        }
    }


}
