using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class AÑADIRrestricciónporPROVINCIAYcódigodemunicipio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_MUNICIPIO_ID_PROVINCIA_CODIGO",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                columns: new[] { "ID_PROVINCIA", "CODIGO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_MUNICIPIO_ID_PROVINCIA_CODIGO",
                schema: "CALLEJERO",
                table: "MUNICIPIO");
        }
    }
}
