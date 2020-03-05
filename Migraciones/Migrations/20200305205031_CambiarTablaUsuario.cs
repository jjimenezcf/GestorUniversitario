using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CambiarTablaUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_EST_ELEMENTO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EST_ELEMENTO",
                schema: "USUARIO",
                table: "EST_ELEMENTO");

            migrationBuilder.RenameTable(
                name: "EST_ELEMENTO",
                schema: "USUARIO",
                newName: "USUARIO",
                newSchema: "USUARIO");

            migrationBuilder.RenameColumn(
                name: "F_INSCRIPCION",
                schema: "USUARIO",
                table: "USUARIO",
                newName: "F_ALTA");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USUARIO",
                schema: "USUARIO",
                table: "USUARIO",
                column: "ID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_USUARIO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USUARIO",
                schema: "USUARIO",
                table: "USUARIO");

            migrationBuilder.RenameTable(
                name: "USUARIO",
                schema: "USUARIO",
                newName: "EST_ELEMENTO",
                newSchema: "USUARIO");

            migrationBuilder.RenameColumn(
                name: "F_ALTA",
                schema: "USUARIO",
                table: "EST_ELEMENTO",
                newName: "F_INSCRIPCION");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EST_ELEMENTO",
                schema: "USUARIO",
                table: "EST_ELEMENTO",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EST_CURSO_EST_ELEMENTO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "EstudianteId",
                principalSchema: "USUARIO",
                principalTable: "EST_ELEMENTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
