using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestor.Elementos.Universitario.Migrations
{
    public partial class VolverAQuitarIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Curso_CursoId",
                table: "Inscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteId",
                table: "Inscripcion");

            migrationBuilder.DropIndex(
                name: "IX_Inscripcion_CursoId",
                table: "Inscripcion");

            migrationBuilder.DropIndex(
                name: "IX_Inscripcion_EstudianteId",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "IdCurso",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "IdEstudiante",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "EstudianteId",
                table: "Inscripcion");

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Inscripcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstudianteId",
                table: "Inscripcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CatalogoDeBd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Catalogo = table.Column<string>(nullable: true),
                    Esquema = table.Column<string>(nullable: true),
                    Tabla = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoDeBd", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_CursoId",
                table: "Inscripcion",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_EstudianteId",
                table: "Inscripcion",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_CursoId",
                table: "Inscripcion",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteId",
                table: "Inscripcion",
                column: "EstudianteId",
                principalTable: "Estudiante",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Curso_CursoId",
                table: "Inscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteId",
                table: "Inscripcion");

            migrationBuilder.DropTable(
                name: "CatalogoDeBd");

            migrationBuilder.DropIndex(
                name: "IX_Inscripcion_CursoId",
                table: "Inscripcion");

            migrationBuilder.DropIndex(
                name: "IX_Inscripcion_EstudianteId",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "EstudianteId",
                table: "Inscripcion");

            migrationBuilder.AddColumn<int>(
                name: "IdCurso",
                table: "Inscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEstudiante",
                table: "Inscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegistroDeCursoId",
                table: "Inscripcion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistroDeEstudianteId",
                table: "Inscripcion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_RegistroDeCursoId",
                table: "Inscripcion",
                column: "RegistroDeCursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_RegistroDeEstudianteId",
                table: "Inscripcion",
                column: "RegistroDeEstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_RegistroDeCursoId",
                table: "Inscripcion",
                column: "RegistroDeCursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Estudiante_RegistroDeEstudianteId",
                table: "Inscripcion",
                column: "RegistroDeEstudianteId",
                principalTable: "Estudiante",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
