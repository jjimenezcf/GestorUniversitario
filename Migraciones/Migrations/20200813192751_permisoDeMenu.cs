using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class permisoDeMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "MENU",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU",
                column: "IDPERMISO",
                unique: true,
                filter: "[IDPERMISO] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU",
                column: "IDPERMISO",
                principalSchema: "SEGURIDAD",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");

            migrationBuilder.DropIndex(
                name: "IX_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");

            migrationBuilder.DropColumn(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");
        }
    }
}
