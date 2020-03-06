using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class AnadirTablaDeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EST_CURSO",
                schema: "UNIVERSIDAD");

            migrationBuilder.AddColumn<bool>(
                name: "PERMISO",
                schema: "PERMISO",
                table: "PERMISO",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ROL",
                schema: "PERMISO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROL_PERMISO",
                schema: "PERMISO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<int>(nullable: false),
                    IdPermiso = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: true),
                    RolRegId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL_PERMISO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROL_PERMISO_PERMISO_CursoId",
                        column: x => x.CursoId,
                        principalSchema: "PERMISO",
                        principalTable: "PERMISO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ROL_PERMISO_ROL_RolRegId",
                        column: x => x.RolRegId,
                        principalSchema: "PERMISO",
                        principalTable: "ROL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROL_PERMISO",
                schema: "PERMISO");

            migrationBuilder.DropTable(
                name: "ROL",
                schema: "PERMISO");

            migrationBuilder.DropColumn(
                name: "PERMISO",
                schema: "PERMISO",
                table: "PERMISO");

            migrationBuilder.EnsureSchema(
                name: "UNIVERSIDAD");

            migrationBuilder.CreateTable(
                name: "EST_CURSO",
                schema: "UNIVERSIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CursoId = table.Column<int>(type: "INT", nullable: false),
                    EstudianteId = table.Column<int>(type: "int", nullable: false),
                    Grado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EST_CURSO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EST_CURSO_PERMISO_CursoId",
                        column: x => x.CursoId,
                        principalSchema: "PERMISO",
                        principalTable: "PERMISO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "CursoId");
        }
    }
}
