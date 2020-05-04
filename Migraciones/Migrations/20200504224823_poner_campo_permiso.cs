using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class poner_campo_permiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");
        }
    }
}
