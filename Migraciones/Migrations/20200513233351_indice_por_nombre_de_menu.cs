using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class indice_por_nombre_de_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_MENU_NOMBRE",
                schema: "ENTORNO",
                table: "MENU",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MENU_NOMBRE",
                schema: "ENTORNO",
                table: "MENU");

        }
    }
}
