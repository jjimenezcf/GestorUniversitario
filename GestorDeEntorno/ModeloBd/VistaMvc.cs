using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("VISTA_MVC", Schema = "ENTORNO")]
    public class VistaMvcDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }
        [Required]
        [Column("CONTROLADOR", TypeName = "VARCHAR(250)")]
        public string Controlador { get; set; }

        [Required]
        [Column("ACCION", TypeName = "VARCHAR(250)")]
        public string Accion { get; set; }

        [Column("PARAMETROS", TypeName = "VARCHAR(250)")]
        public string Parametros { get; set; }

        public List<MenuDtm> Menus { get; set; }
    }

    public static class TablaVistaMvc
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VistaMvcDtm>().Property(menu => menu.Parametros).IsRequired(false);

            modelBuilder.Entity<VistaMvcDtm>()
               .HasIndex(vista => new { vista.Controlador, vista.Accion, vista.Parametros })
               .IsUnique(true)
               .HasName("IX_VISTA_MVC");

            modelBuilder.Entity<VistaMvcDtm>()
                .HasMany(vista => vista.Menus)
                .WithOne(vista => vista.VistaMvc)
                .HasForeignKey(menu => menu.IdVistaMvc);
        }
    }
}
