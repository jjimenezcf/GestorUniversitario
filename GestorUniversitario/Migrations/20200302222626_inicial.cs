using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorUniversitario.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    Creditos = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Estudiante",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellido = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    InscritoEl = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiante", x => x.ID);
                });

            //migrationBuilder.CreateTable(
            //    name: "VAR_ELEMENTO",
            //    schema: "ENTORNO",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "INT", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NOMBRE = table.Column<string>(type: "VARCHAR(50)", nullable: false),
            //        DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
            //        VALOR = table.Column<string>(type: "VARCHAR(50)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_VAR_ELEMENTO", x => x.ID);
            //    });

            migrationBuilder.CreateTable(
                name: "Inscripcion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CursoId = table.Column<int>(nullable: false),
                    EstudianteId = table.Column<int>(nullable: false),
                    Grado = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripcion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Estudiante_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiante",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_CursoId",
                table: "Inscripcion",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_EstudianteId",
                table: "Inscripcion",
                column: "EstudianteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscripcion");

            //migrationBuilder.DropTable(
            //    name: "VAR_ELEMENTO",
            //    schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "Estudiante");
        }
    }
}
