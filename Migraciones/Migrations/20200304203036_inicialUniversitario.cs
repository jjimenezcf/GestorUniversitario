using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class inicialUniversitario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UNIVERSIDAD");

            migrationBuilder.CreateTable(
                name: "CUR_ELEMENTO",
                schema: "UNIVERSIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TITULO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CREDITOS = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUR_ELEMENTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EST_ELEMENTO",
                schema: "UNIVERSIDAD",
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
                    table.PrimaryKey("PK_EST_ELEMENTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EST_CURSO",
                schema: "UNIVERSIDAD",
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
                    table.PrimaryKey("PK_EST_CURSO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EST_CURSO_CUR_ELEMENTO_CursoId",
                        column: x => x.CursoId,
                        principalSchema: "UNIVERSIDAD",
                        principalTable: "CUR_ELEMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EST_CURSO_EST_ELEMENTO_EstudianteId",
                        column: x => x.EstudianteId,
                        principalSchema: "UNIVERSIDAD",
                        principalTable: "EST_ELEMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_EstudianteId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "EstudianteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EST_CURSO",
                schema: "UNIVERSIDAD");

            migrationBuilder.DropTable(
                name: "CUR_ELEMENTO",
                schema: "UNIVERSIDAD");

            migrationBuilder.DropTable(
                name: "EST_ELEMENTO",
                schema: "UNIVERSIDAD");
        }
    }
}
