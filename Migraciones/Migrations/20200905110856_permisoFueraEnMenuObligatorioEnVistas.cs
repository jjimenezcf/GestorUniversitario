using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class permisoFueraEnMenuObligatorioEnVistas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");

            migrationBuilder.DropIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.DropIndex(
                name: "IX_MENU_IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");

            migrationBuilder.DropColumn(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "MENU");

            migrationBuilder.AlterColumn<int>(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "IDPERMISO",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.AlterColumn<int>(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                type: "INT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AddColumn<int>(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "MENU",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "IDPERMISO",
                unique: true,
                filter: "[IDPERMISO] IS NOT NULL");

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
    }
}
