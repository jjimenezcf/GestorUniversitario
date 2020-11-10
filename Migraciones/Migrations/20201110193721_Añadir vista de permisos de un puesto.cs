using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class Añadirvistadepermisosdeunpuesto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

            CREATE FUNCTION SEGURIDAD.OBTENER_ORIGEN_PUESTO_PERMISO (
            	@idPuesto int,
            	@idPermiso int
            )
            RETURNS VarChar(max)
            AS
            begin
            Declare @origen varchar(max)
            Declare @resultado varchar(max)
            DECLARE c CURSOR FOR SELECT t2.NOMBRE
                      from SEGURIDAD.ROL_PUESTO T1
                      inner join seguridad.rol t2 on t2.id = t1.IDROL 
                      INNER JOIN SEGURIDAD.ROL_PERMISO T3 ON T3.IDROL = T2.ID
                      where t1.IDPUESTO = @idPuesto and t3.IDPERMISO = @idPermiso
             set @resultado = ''
            OPEN c
            FETCH NEXT FROM c INTO @origen
            WHILE @@fetch_status = 0
            BEGIN
			    if @resultado = ''  
                   set  @resultado = @origen 
				else 
                   set  @resultado = @resultado + ' - ' + @origen 
                FETCH NEXT FROM c INTO @origen
            END
            CLOSE c
            DEALLOCATE c
            
            return @resultado
            
            END
            go

            CREATE VIEW[SEGURIDAD].[PUESTO_PERMISO]
            AS
            select CAST(ROW_NUMBER() OVER(ORDER BY t1.IDPUESTO ASC) as int) AS ID
            , t1.IDPUESTO as IDPUESTO
            , t2.IDPERMISO as IDPERMISO
            , SEGURIDAD.OBTENER_ORIGEN_PUESTO_PERMISO(IDPUESTO, IDPERMISO) as ROLES
            from SEGURIDAD.ROL_PUESTO T1 
            INNER JOIN SEGURIDAD.ROL_PERMISO T2 ON T2.IDROL = T1.IDROL
            group by T1.IDPUESTO, T2.IDPERMISO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW [SEGURIDAD].[PUESTO_PERMISO]
		    GO
		    
		    DROP FUNCTION SEGURIDAD.OBTENER_ORIGEN_PUESTO_PERMISO
		    gO
            ");
        }
    }
}
