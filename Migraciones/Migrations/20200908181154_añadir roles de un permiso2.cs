using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirrolesdeunpermiso2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                     create view SEGURIDAD.PERMISO_ROL AS SELECT * FROM SEGURIDAD.ROL_PERMISO
                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql($@"
                     DROP view SEGURIDAD.PERMISO_ROL
                    ");
        }
    }
}
