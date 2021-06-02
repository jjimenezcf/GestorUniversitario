using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class quitarrestricciónporcódigodemunicipio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MUNICIPIO_CODIGO",
                schema: "CALLEJERO",
                table: "MUNICIPIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_MUNICIPIO_CODIGO",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                column: "CODIGO");
        }
    }
}
