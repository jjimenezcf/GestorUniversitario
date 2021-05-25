using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;

namespace ServicioDeDatos.Entorno
{
    [Table("VISTA_MVC", Schema = "ENTORNO")]
    public class VistaMvcDtm : RegistroConNombre
    {
        [Required]
        [Column("CONTROLADOR", TypeName = "VARCHAR(250)")]
        public string Controlador { get; set; }

        [Required]
        [Column("ACCION", TypeName = "VARCHAR(250)")]
        public string Accion { get; set; }

        [Column("PARAMETROS", TypeName = "VARCHAR(250)")]
        public string Parametros { get; set; }

        [Column("MODAL", TypeName ="BIT")]
        public bool MostrarEnModal { get; set; }

        public List<MenuDtm> Menus { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }
        
        public PermisoDtm Permiso { get; set; }

        public string ElementoDto { get; set; }

    }
    public static class VistaMvcSqls
    {
        public static readonly string LeerVistaPorDto = @"
SELECT [ID]
      ,[NOMBRE]
      ,[CONTROLADOR]
      ,[ACCION]
      ,[PARAMETROS]
      ,[MODAL]
      ,[IDPERMISO]
      ,[ELEMENTO_DTO] as ElementoDto
FROM [ENTORNO].[VISTA_MVC]
WHERE [ELEMENTO_DTO] = @ElementoDto
";
    }
    public static class TablaVistaMvc
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VistaMvcDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();

            modelBuilder.Entity<VistaMvcDtm>().Property(p => p.Parametros).IsRequired(false);

            modelBuilder.Entity<VistaMvcDtm>().Property(p => p.ElementoDto).HasColumnName("ELEMENTO_DTO").HasColumnType("VARCHAR(250)").IsRequired(false);

            modelBuilder.Entity<VistaMvcDtm>().Property(p => p.MostrarEnModal).IsRequired(true).HasDefaultValue(false);

            //modelBuilder.Entity<VistaMvcDtm>().Property(p => p.IdPermiso).IsRequired(false);

            modelBuilder.Entity<VistaMvcDtm>()
                .HasOne(p => p.Permiso)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(p => p.IdPermiso)
                .HasConstraintName("FK_VISTA_MVC_IDPERMISO")
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<VistaMvcDtm>()
               .HasIndex(vista => new { vista.IdPermiso})
               .IsUnique(true)
               .HasDatabaseName("IX_VISTA_MVC_IDPERMISO");

            modelBuilder.Entity<VistaMvcDtm>()
               .HasIndex(vista => new { vista.Controlador, vista.Accion, vista.Parametros })
               .IsUnique(true)
               .HasDatabaseName("IX_VISTA_MVC");

            modelBuilder.Entity<VistaMvcDtm>()
                .HasMany(vista => vista.Menus)
                .WithOne(vista => vista.VistaMvc)
                .HasForeignKey(menu => menu.IdVistaMvc);

            modelBuilder.Entity<VistaMvcDtm>().HasIndex(v => new { v.Nombre }).IsUnique(true).HasDatabaseName("IND_VISTAMVC_NOMBRE");
        }
    }


}
