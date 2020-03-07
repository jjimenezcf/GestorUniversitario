using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class anadirAk_Permiso_Nombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PERMISO_NOMBRE",
                schema: "PERMISO",
                table: "PERMISO",
                column: "NOMBRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PERMISO_NOMBRE",
                schema: "PERMISO",
                table: "PERMISO");
        }
    }
}
