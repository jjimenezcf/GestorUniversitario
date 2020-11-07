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

    drop FUNCTION SEGURIDAD.OBTENER_ORIGEN
     go
     
     CREATE FUNCTION SEGURIDAD.OBTENER_ORIGEN (
     	@idUsuario int,
     	@idPermiso int
     )
     RETURNS VarChar(max)
     AS
     begin
     Declare @origen varchar(max)
     Declare @resultado varchar(max)
     DECLARE c CURSOR FOR SELECT ' Puesto: ' + t4.NOMBRE + ' Rol: ' + t6.NOMBRE
      from SEGURIDAD.USU_PUESTO t2
      inner join SEGURIDAD.ROL_PUESTO T3 ON T3.IDPUESTO = T2.IDPUESTO
      inner join SEGURIDAD.PUESTO t4 on t4.id = t3.IDPUESTO
      INNER JOIN SEGURIDAD.ROL_PERMISO T5 ON T5.IDROL = T3.IDROL
      inner join SEGURIDAD.ROL t6 on t6.ID = t5.IDROL
      inner join SEGURIDAD.PERMISO t7 on t7.id = t5.IDPERMISO
      where idusua = @idUsuario and t7.ID = @idPermiso
      set @resultado = ''
     OPEN c
     FETCH NEXT FROM c INTO @origen
     WHILE @@fetch_status = 0
     BEGIN
         set  @resultado = @resultado + char(10) + @origen 
         FETCH NEXT FROM c INTO @origen
     END
     CLOSE c
     DEALLOCATE c
     
     return @resultado
     
     END
     GO
     
     select *, SEGURIDAD.OBTENER_ORIGEN(IDUSUA,IDPERMISO) from ENTORNO.USU_PERMISO  where IDUSUA = 7   
     
     
     */


    public class PermisosDeUnUsuarioDtm : Registro
    {
        [Required]
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public string Origen { get; set; }

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

            modelBuilder.Entity<PermisosDeUnUsuarioDtm>().Property(p => p.Origen).HasColumnName("ORIGEN").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("SEGURIDAD.OBTENER_ORIGEN(idusua,idpermiso)");

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
