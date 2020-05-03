using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class AnadirVistaUsuPermiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" CREATE VIEW ENTORNO.USU_PERMISO
                                    AS
                                    select ROW_NUMBER() OVER(ORDER BY t2.IDUSUA ASC) AS ID, t2.IDUSUA as IDUSUA, t4.IDPERMISO as IDPERMISO
                                    from SEGURIDAD.USU_PUESTO t2
                                    inner
                                    join SEGURIDAD.ROL_PUESTO T3 ON T3.IDPUESTO = T2.IDPUESTO
                                    INNER JOIN SEGURIDAD.ROL_PERMISO T4 ON T4.IDROL = T3.IDROL
                                    group by IDUSUA, IDPERMISO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW ENTORNO.USU_PERMISO");
        }
    }
}
