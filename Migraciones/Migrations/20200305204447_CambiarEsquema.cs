using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CambiarEsquema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "USUARIO");

            migrationBuilder.RenameTable(
                name: "EST_ELEMENTO",
                schema: "UNIVERSIDAD",
                newName: "EST_ELEMENTO",
                newSchema: "USUARIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "EST_ELEMENTO",
                schema: "USUARIO",
                newName: "EST_ELEMENTO",
                newSchema: "UNIVERSIDAD");
        }
    }
}
