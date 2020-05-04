using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class quitar_campo_permiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "I_PERMISO_IDCLASE_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "IDCLASE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "I_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.AddColumn<string>(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_IDCLASE_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                columns: new[] { "IDCLASE", "PERMISO" },
                unique: true);
        }
    }
}
