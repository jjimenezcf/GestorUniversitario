using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificarindicesdetrabajossometidos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TRABAJO_METODO",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.DropIndex(
                name: "IX_TRABAJO_PA",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_METODO",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "NOMBRE", "DLL", "CLASE", "METODO" },
                unique: true,
                filter: "[DLL] IS NOT NULL AND [CLASE] IS NOT NULL AND [METODO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_PA",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "NOMBRE", "ESQUEMA", "PA" },
                unique: true,
                filter: "[ESQUEMA] IS NOT NULL AND [PA] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TRABAJO_METODO",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.DropIndex(
                name: "IX_TRABAJO_PA",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_METODO",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "DLL", "CLASE", "METODO" },
                unique: true,
                filter: "[DLL] IS NOT NULL AND [CLASE] IS NOT NULL AND [METODO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_PA",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "ESQUEMA", "PA" },
                unique: true,
                filter: "[ESQUEMA] IS NOT NULL AND [PA] IS NOT NULL");
        }
    }
}
