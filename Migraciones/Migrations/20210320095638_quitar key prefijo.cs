using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class quitarkeyprefijo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PROVINCIA_PREFIJO",
                schema: "CALLEJERO",
                table: "PROVINCIA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PROVINCIA_PREFIJO",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                column: "PREFIJO");
        }
    }
}
