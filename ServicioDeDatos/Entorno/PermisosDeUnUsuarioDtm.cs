using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Entorno
{
    /*
     DROP VIEW [ENTORNO].[USU_PERMISO]
     GO
     
     CREATE VIEW[ENTORNO].[USU_PERMISO]
     AS
     select CAST(ROW_NUMBER() OVER(ORDER BY t2.IDUSUA ASC) as int) AS ID, t2.IDUSUA as IDUSUA, t4.IDPERMISO as IDPERMISO
     from SEGURIDAD.USU_PUESTO t2
     inner join SEGURIDAD.ROL_PUESTO T3 ON T3.IDPUESTO = T2.IDPUESTO
     INNER JOIN SEGURIDAD.ROL_PERMISO T4 ON T4.IDROL = T3.IDROL
     group by IDUSUA, IDPERMISO
     GO 
     
     */


    public class PermisosDeUnUsuarioDtm : Registro
    {
        [Required]
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public virtual UsuarioDtm Usuario { get; set; }

        public virtual PermisoDtm Permiso { get; set; }
    }

    public static class VistaUsuarioPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisosDeUnUsuarioDtm>()
                .ToTable("USU_PERMISO", "ENTORNO")
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<PermisosDeUnUsuarioDtm>()
                .HasOne(x => x.Usuario)
                .WithMany(x => x.Permisos)
                .HasForeignKey(x => x.IdUsuario);

            modelBuilder.Entity<PermisosDeUnUsuarioDtm>()
                .HasOne(x => x.Permiso)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.IdUsuario);
        }
    }
}
