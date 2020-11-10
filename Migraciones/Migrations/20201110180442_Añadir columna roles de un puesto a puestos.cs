using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class Añadircolumnarolesdeunpuestoapuestos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ROLES",
                schema: "SEGURIDAD",
                table: "PUESTO",
                type: "VARCHAR(MAX)",
                nullable: true,
                computedColumnSql: "SEGURIDAD.OBTENER_ROLES_DE_UN_PUESTO(id)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ROLES",
                schema: "SEGURIDAD",
                table: "PUESTO");
        }
    }
}
