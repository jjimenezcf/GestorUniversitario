using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestor.Elementos.Universitario.Migrations
{
    public partial class CambioObjetoInscripcio_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteID",
                table: "Inscripcion");

            migrationBuilder.RenameColumn(
                name: "EstudianteID",
                table: "Inscripcion",
                newName: "EstudianteId");

            migrationBuilder.RenameColumn(
                name: "CursoID",
                table: "Inscripcion",
                newName: "CursoId");

            migrationBuilder.RenameIndex(
                name: "IX_Inscripcion_EstudianteID",
                table: "Inscripcion",
                newName: "IX_Inscripcion_EstudianteId");

            migrationBuilder.RenameIndex(
                name: "IX_Inscripcion_CursoID",
                table: "Inscripcion",
                newName: "IX_Inscripcion_CursoId");

            migrationBuilder.AlterColumn<int>(
                name: "EstudianteId",
                table: "Inscripcion",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CursoId",
                table: "Inscripcion",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdCurso",
                table: "Inscripcion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEstudiante",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_CursoId",
                table: "Inscripcion",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteId",
                table: "Inscripcion",
                column: "EstudianteId",
                principalTable: "Estudiante",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropColumn(
                name: "IdCurso",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "IdEstudiante",
                table: "Inscripcion");

            migrationBuilder.RenameColumn(
                name: "EstudianteId",
                table: "Inscripcion",
                newName: "EstudianteID");

            migrationBuilder.RenameColumn(
                name: "CursoId",
                table: "Inscripcion",
                newName: "CursoID");

            migrationBuilder.RenameIndex(
                name: "IX_Inscripcion_EstudianteId",
                table: "Inscripcion",
                newName: "IX_Inscripcion_EstudianteID");

            migrationBuilder.RenameIndex(
                name: "IX_Inscripcion_CursoId",
                table: "Inscripcion",
                newName: "IX_Inscripcion_CursoID");

            migrationBuilder.AlterColumn<int>(
                name: "EstudianteID",
                table: "Inscripcion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CursoID",
                table: "Inscripcion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Curso_CursoID",
                table: "Inscripcion",
                column: "CursoID",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Estudiante_EstudianteID",
                table: "Inscripcion",
                column: "EstudianteID",
                principalTable: "Estudiante",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
