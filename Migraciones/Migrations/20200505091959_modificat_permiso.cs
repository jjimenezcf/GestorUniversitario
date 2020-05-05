using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class modificat_permiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "I_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.AddColumn<int>(
                name: "IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql($@"update SEGURIDAD.PERMISO set idtipo = 2 where nombre like '%consultor%'
                                    go
                                    update SEGURIDAD.PERMISO set idtipo = 1 where nombre like '%gestor%'
                                    go");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISO_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "IDTIPO");

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_IDCLASE_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                columns: new[] { "IDCLASE", "IDTIPO" });

            migrationBuilder.AddForeignKey(
                name: "FK_PERMISO_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "IDTIPO",
                principalSchema: "SEGURIDAD",
                principalTable: "TIPO_PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PERMISO_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropIndex(
                name: "IX_PERMISO_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropIndex(
                name: "I_PERMISO_IDCLASE_IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "IDTIPO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.AddColumn<string>(
                name: "PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "IDCLASE");
        }
    }
}
