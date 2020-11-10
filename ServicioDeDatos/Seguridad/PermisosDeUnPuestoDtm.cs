using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{
    public class PermisosDeUnPuestoDtm : Registro
    {
        [Required]
        [Column("IDPUESTO", TypeName = "INT")]
        public int IdPuesto { get; set; }

        [Required]
        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public string Roles { get; set; }

        public virtual PuestoDtm Puesto { get; set; }

        public virtual PermisoDtm Permiso { get; set; }
    }

    public static class VistaDePermisosDeUnPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisosDeUnPuestoDtm>()
                .ToView("PUESTO_PERMISO", "ENTORNO")
                .HasKey(x => new { x.Id });


            modelBuilder.Entity<PermisosDeUnPuestoDtm>().Property(p => p.Id).HasColumnName("ID").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("CAST(ROW_NUMBER() OVER(ORDER BY t2.IDUSUA ASC) as int)");
            modelBuilder.Entity<PermisosDeUnPuestoDtm>().Property(p => p.Roles).HasColumnName("ROLES").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("SEGURIDAD.OBTENER_ORIGEN_PUESTO_PERMISO(IDPUESTO, IDPERMISO)");

        }
    }
}
