using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class quitarIndiceUnicoAlNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "I_ARCHIVO_NOMBRE",
                schema: "SISDOC",
                table: "ARCHIVO");

            migrationBuilder.CreateIndex(
                name: "I_ARCHIVO_NOMBRE",
                schema: "SISDOC",
                table: "ARCHIVO",
                column: "NOMBRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "I_ARCHIVO_NOMBRE",
                schema: "SISDOC",
                table: "ARCHIVO");

            migrationBuilder.CreateIndex(
                name: "I_ARCHIVO_NOMBRE",
                schema: "SISDOC",
                table: "ARCHIVO",
                column: "NOMBRE",
                unique: true);
        }
    }
}
