using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Entorno
{
    [Table("MENU_SE", Schema = "ENTORNO")]
    public class ArbolDeMenuDtm : Registro
    {
        [Column("PADRE", TypeName = "VARCHAR(250)")]
        public string Padre { get; set; }

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


        [Column("IDVISTA_MVC", TypeName = "INT")]
        public int? IdVistaMvc { get; set; }

        [Column("VISTA", TypeName = "VARCHAR(250)")]
        public string Vista { get; set; }

        [Column("CONTROLADOR", TypeName = "VARCHAR(250)")]
        public string Controlador { get; set; }

        [Column("ACCION", TypeName = "VARCHAR(250)")]
        public string accion { get; set; }

        [Column("PARAMETROS", TypeName = "VARCHAR(250)")]
        public string parametros { get; set; }
    }

    public static class VistaMenuSe
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArbolDeMenuDtm>()
                    .ToTable("MENU_SE", "ENTORNO")
                    .HasKey(x => new {
                        x.Id
                    });


            modelBuilder.Entity<ArbolDeMenuDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();

            modelBuilder.Entity<ArbolDeMenuDtm>().Property(menu => menu.IdPadre).IsRequired(false);
            modelBuilder.Entity<ArbolDeMenuDtm>().Property(menu => menu.IdVistaMvc).IsRequired(false);
        }
    }

}
