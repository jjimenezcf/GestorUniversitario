using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class indicepornombredevariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.CreateIndex(
                name: "IND_NOMBRE",
                schema: "ENTORNO",
                table: "VARIABLE",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IND_NOMBRE",
                schema: "ENTORNO",
                table: "VARIABLE");

            migrationBuilder.CreateIndex(
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "NOMBRE",
                unique: true);
        }
    }
}
