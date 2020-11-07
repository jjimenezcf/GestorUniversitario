using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirorigendeunpermiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            DROP VIEW [ENTORNO].[USU_PERMISO]
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
            DECLARE c CURSOR FOR SELECT t4.NOMBRE + ' (Rol: ' + t6.NOMBRE + ')'
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
            GO
            
            CREATE VIEW[ENTORNO].[USU_PERMISO]
            AS
            select CAST(ROW_NUMBER() OVER(ORDER BY t2.IDUSUA ASC) as int) AS ID, t2.IDUSUA as IDUSUA, t4.IDPERMISO as IDPERMISO, SEGURIDAD.OBTENER_ORIGEN (t2.IDUSUA, t4.IDPERMISO) as ORIGEN
            from SEGURIDAD.USU_PUESTO t2
            inner join SEGURIDAD.ROL_PUESTO T3 ON T3.IDPUESTO = T2.IDPUESTO
            INNER JOIN SEGURIDAD.ROL_PERMISO T4 ON T4.IDROL = T3.IDROL
            group by IDUSUA, IDPERMISO
            GO 
        
            
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
