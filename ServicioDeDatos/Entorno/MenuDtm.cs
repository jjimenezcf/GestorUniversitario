﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Entorno
{
    [Table("MENU", Schema = "ENTORNO")]
    public class MenuDtm : RegistroConNombre
    {
        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("ICONO", TypeName = "VARCHAR(250)")]
        public string Icono { get; set; }

        [Required]
        [Column("ORDEN", TypeName = "INT")]
        public int Orden { get; set; }

        [Required]
        [Column("ACTIVO", TypeName = "BIT")]
        public bool Activo { get; set; }

        [Column("IDPADRE", TypeName = "INT")]
        public int? IdPadre { get; set; }

        public MenuDtm Padre { get; set; }

        public List<MenuDtm> Submenus { get; set; }

        [Column("IDVISTA_MVC", TypeName = "INT")]
        public int? IdVistaMvc { get; set; }

        public VistaMvcDtm VistaMvc { get; set; }
    }

    public static class TablaMenu
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<MenuDtm>().Property(menu => menu.IdPadre).IsRequired(false);
            modelBuilder.Entity<MenuDtm>().Property(menu => menu.IdVistaMvc).IsRequired(false);

            modelBuilder.Entity<MenuDtm>().Property(menu => menu.Orden).HasDefaultValue(0);

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

            modelBuilder.Entity<MenuDtm>().HasIndex(x => x.Nombre).IsUnique();
        }
    }


}
