using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class EvitarBorradosEnCascada : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PUESTO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO");

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO");

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDPERMISO",
                principalSchema: "SEGURIDAD",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDROL",
                principalSchema: "SEGURIDAD",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PUESTO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO",
                column: "IDROL",
                principalSchema: "SEGURIDAD",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO",
                column: "IDPUESTO",
                principalSchema: "SEGURIDAD",
                principalTable: "PUESTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PUESTO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO");

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PUESTO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO",
                column: "IDROL",
                principalSchema: "SEGURIDAD",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO",
                column: "IDPUESTO",
                principalSchema: "SEGURIDAD",
                principalTable: "PUESTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
