using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CREAR_TABLA_ROL_pERMISO2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDPERMISO");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDROL");

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDPERMISO",
                principalSchema: "SEGURIDAD",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDROL",
                principalSchema: "SEGURIDAD",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.AddColumn<int>(
                name: "PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "PermisoId",
                principalSchema: "SEGURIDAD",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "RolesId",
                principalSchema: "SEGURIDAD",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
