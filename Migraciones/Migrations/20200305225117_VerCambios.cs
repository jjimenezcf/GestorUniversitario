using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class VerCambios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_USUARIO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropIndex(
                name: "IX_EST_CURSO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.AlterColumn<int>(
                name: "EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_EST_CURSO_USUARIO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "UsuarioId",
                principalSchema: "USUARIO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_USUARIO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropIndex(
                name: "IX_EST_CURSO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.AlterColumn<int>(
                name: "EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_EST_CURSO_USUARIO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "EstudianteId",
                principalSchema: "USUARIO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
