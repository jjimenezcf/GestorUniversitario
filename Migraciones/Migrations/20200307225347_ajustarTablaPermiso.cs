using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class ajustarTablaPermiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TIENE",
                schema: "PERMISO",
                table: "PERMISO");

            migrationBuilder.AlterColumn<decimal>(
                name: "PERMISO",
                schema: "PERMISO",
                table: "PERMISO",
                type: "decimal(2,0)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AlterColumn<decimal>(
                name: "CLASE",
                schema: "PERMISO",
                table: "PERMISO",
                type: "decimal(2,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PERMISO",
                schema: "PERMISO",
                table: "PERMISO",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,0)");

            migrationBuilder.AlterColumn<int>(
                name: "CLASE",
                schema: "PERMISO",
                table: "PERMISO",
                type: "INT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,0)");

            migrationBuilder.AddColumn<bool>(
                name: "TIENE",
                schema: "PERMISO",
                table: "PERMISO",
                type: "BIT",
                nullable: false,
                defaultValue: false);
        }
    }
}
