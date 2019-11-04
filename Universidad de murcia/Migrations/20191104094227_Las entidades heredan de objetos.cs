using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversidadDeMurcia.Migrations
{
    public partial class Lasentidadesheredandeobjetos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscripcion",
                table: "Inscripcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Curso",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "InscripcionID",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "CursoID",
                table: "Curso");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Inscripcion",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Curso",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscripcion",
                table: "Inscripcion",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Curso",
                table: "Curso",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion",
                column: "CursoID",
                principalTable: "Curso",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscripcion",
                table: "Inscripcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Curso",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Curso");

            migrationBuilder.AddColumn<int>(
                name: "InscripcionID",
                table: "Inscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CursoID",
                table: "Curso",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscripcion",
                table: "Inscripcion",
                column: "InscripcionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Curso",
                table: "Curso",
                column: "CursoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion",
                column: "CursoID",
                principalTable: "Curso",
                principalColumn: "CursoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
