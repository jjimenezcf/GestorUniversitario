using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificacionesentabladepais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NAME",
                schema: "CALLEJERO",
                table: "PAIS",
                newName: "NOMBRE_INGLES");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PAIS_CODIGO",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "CODIGO");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PAIS_ISO2",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "ISO2");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PAIS_NAME",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "NOMBRE_INGLES");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PAIS_CODIGO",
                schema: "CALLEJERO",
                table: "PAIS");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PAIS_ISO2",
                schema: "CALLEJERO",
                table: "PAIS");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PAIS_NAME",
                schema: "CALLEJERO",
                table: "PAIS");

            migrationBuilder.RenameColumn(
                name: "NOMBRE_INGLES",
                schema: "CALLEJERO",
                table: "PAIS",
                newName: "NAME");
        }
    }
}
