using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class quitarrestriccionenmunicipiocp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MUNICIPIO_CP_ID_CP",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MUNICIPIO_CP",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP",
                columns: new[] { "ID_CP", "ID_MUNICIPIO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MUNICIPIO_CP",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MUNICIPIO_CP_ID_CP",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP",
                column: "ID_CP");
        }
    }
}
