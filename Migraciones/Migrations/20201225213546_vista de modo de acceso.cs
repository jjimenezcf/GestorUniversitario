using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class vistademododeacceso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                     CREATE FUNCTION [NEGOCIO].[MODO_ACCESO_AL_NEGOCIO_POR_USUARIO] 
                     (
                      @NEGOCIO VARCHAR(250),
                      @IDUSUARIO INT
                     )
                     RETURNS TABLE 
                     AS
                     RETURN 
                     (
                          select t1.ID, t1.ADMINISTRADOR, t1.GESTOR, t1.CONSULTOR, t2.IDUSUA, t2.IDPERMISO, t2.ORIGEN
                          from (
                          SELECT t1.ID, cast(1 as bit) as Administrador,  cast(0 as bit) as Gestor, cast(0 as bit) as Consultor
                          FROM ENTORNO.USU_PERMISO t1 
                          inner join NEGOCIO.NEGOCIO t2 on t2.IDPERMISO_ADMINISTRADOR = t1.IDPERMISO 
                          where t1.IDUSUA =  @IDUSUARIO
                            and t2.NOMBRE = @NEGOCIO
                          UNION 
                          SELECT t1.ID, cast(0 as bit) as Administrador,  cast(1 as bit) as Gestor, cast(0 as bit) as Consultor
                          FROM ENTORNO.USU_PERMISO t1 
                          inner join NEGOCIO.NEGOCIO t2 on t2.IDPERMISO_GESTOR = t1.IDPERMISO 
                          where t1.IDUSUA =  @IDUSUARIO
                            and t2.NOMBRE = @NEGOCIO
                          UNION 
                          SELECT  t1.ID,  cast(0 as bit) as Administrador, cast(0 as bit) as Gestor,  cast(1 as bit) as Consultor
                          FROM ENTORNO.USU_PERMISO t1 
                          inner join NEGOCIO.NEGOCIO t2 on t2.IDPERMISO_CONSULTOR = t1.IDPERMISO 
                          where t1.IDUSUA =  @IDUSUARIO
                            and t2.NOMBRE = @NEGOCIO
                            ) t1
                          inner join ENTORNO.USU_PERMISO t2 on t2.ID = t1.ID
                     )
                     ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION [NEGOCIO].[MODO_ACCESO_AL_NEGOCIO_POR_USUARIO]");
        }
    }
}
