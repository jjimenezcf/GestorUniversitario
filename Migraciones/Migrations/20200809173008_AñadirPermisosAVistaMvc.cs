using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class AñadirPermisosAVistaMvc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "IDPERMISO",
                unique: true,
                filter: "[IDPERMISO] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "IDPERMISO",
                principalSchema: "SEGURIDAD",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.DropIndex(
                name: "IX_VISTA_MVC_IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC");

            migrationBuilder.DropColumn(
                name: "IDPERMISO",
                schema: "ENTORNO",
                table: "VISTA_MVC");
        }
    }
}
