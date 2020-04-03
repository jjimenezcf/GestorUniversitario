using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class modificarCamposDePermisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"ALTER TABLE[SEGURIDAD].[PERMISO] DROP CONSTRAINT[AK_PERMISO_PERMISO]");
            migrationBuilder.AlterColumn<string>(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,0)");

            migrationBuilder.AlterColumn<string>(
                name: "CLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "decimal(2,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "decimal(2,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)");
        }
    }
}
