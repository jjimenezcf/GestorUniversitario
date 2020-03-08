using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CREAR_TABLA_ROL_pERMISO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_PERMISO_CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.DropForeignKey(
                name: "FK_ROL_PERMISO_ROL_RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO");

            migrationBuilder.EnsureSchema(
                name: "SEGURIDAD");

            migrationBuilder.RenameTable(
                name: "ROL_PERMISO",
                schema: "PERMISO",
                newName: "ROL_PERMISO",
                newSchema: "SEGURIDAD");

            migrationBuilder.RenameTable(
                name: "ROL",
                schema: "PERMISO",
                newName: "ROL",
                newSchema: "SEGURIDAD");

            migrationBuilder.RenameTable(
                name: "PERMISO",
                schema: "PERMISO",
                newName: "PERMISO",
                newSchema: "SEGURIDAD");

            migrationBuilder.RenameColumn(
                name: "IdRol",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                newName: "IDROL");

            migrationBuilder.RenameColumn(
                name: "IdPermiso",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                newName: "IDPERMISO");

            migrationBuilder.AlterColumn<int>(
                name: "IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IDPERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descri",
                schema: "SEGURIDAD",
                table: "ROL",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ROL_NOMBRE",
                schema: "SEGURIDAD",
                table: "ROL",
                column: "NOMBRE");

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
                name: "IX_ROL_PERMISO_PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ROL_NOMBRE",
                schema: "SEGURIDAD",
                table: "ROL");

            migrationBuilder.DropColumn(
                name: "PermisoId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "RolesId",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "Descri",
                schema: "SEGURIDAD",
                table: "ROL");

            migrationBuilder.EnsureSchema(
                name: "PERMISO");

            migrationBuilder.RenameTable(
                name: "ROL_PERMISO",
                schema: "SEGURIDAD",
                newName: "ROL_PERMISO",
                newSchema: "PERMISO");

            migrationBuilder.RenameTable(
                name: "ROL",
                schema: "SEGURIDAD",
                newName: "ROL",
                newSchema: "PERMISO");

            migrationBuilder.RenameTable(
                name: "PERMISO",
                schema: "SEGURIDAD",
                newName: "PERMISO",
                newSchema: "PERMISO");

            migrationBuilder.RenameColumn(
                name: "IDROL",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                newName: "IdRol");

            migrationBuilder.RenameColumn(
                name: "IDPERMISO",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                newName: "IdPermiso");

            migrationBuilder.AlterColumn<int>(
                name: "IdRol",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AlterColumn<int>(
                name: "IdPermiso",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                column: "RolRegId");

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_PERMISO_CursoId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                column: "CursoId",
                principalSchema: "PERMISO",
                principalTable: "PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ROL_PERMISO_ROL_RolRegId",
                schema: "PERMISO",
                table: "ROL_PERMISO",
                column: "RolRegId",
                principalSchema: "PERMISO",
                principalTable: "ROL",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
