using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class indices_por_nombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IND_NOMBRE",
                schema: "ENTORNO",
                table: "VARIABLE",
                newName: "IND_VARIABLE_NOMBRE");

            migrationBuilder.CreateIndex(
                name: "IND_VISTAMVC_NOMBRE",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IND_VISTAMVC_NOMBRE",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.RenameIndex(
                name: "IND_VARIABLE_NOMBRE",
                schema: "ENTORNO",
                table: "VARIABLE",
                newName: "IND_NOMBRE");
        }
    }
}
