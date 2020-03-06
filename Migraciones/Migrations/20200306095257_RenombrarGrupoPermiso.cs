using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class RenombrarGrupoPermiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_EST_CURSO_GRUPO_CursoId",
            //    schema: "UNIVERSIDAD",
            //    table: "EST_CURSO");

            migrationBuilder.DropTable(
                name: "GRUPO",
                schema: "USUARIO");

            migrationBuilder.EnsureSchema(
                name: "PERMISO");

            migrationBuilder.CreateTable(
                name: "PERMISO",
                schema: "PERMISO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CLASE = table.Column<int>(type: "INT", nullable: false),
                    TIENE = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISO", x => x.ID);
                });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EST_CURSO_PERMISO_CursoId",
            //    schema: "UNIVERSIDAD",
            //    table: "EST_CURSO",
            //    column: "CursoId",
            //    principalSchema: "PERMISO",
            //    principalTable: "PERMISO",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_PERMISO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropTable(
                name: "PERMISO",
                schema: "PERMISO");

            migrationBuilder.CreateTable(
                name: "GRUPO",
                schema: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLASE = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRUPO", x => x.ID);
                });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EST_CURSO_GRUPO_CursoId",
            //    schema: "UNIVERSIDAD",
            //    table: "EST_CURSO",
            //    column: "CursoId",
            //    principalSchema: "USUARIO",
            //    principalTable: "GRUPO",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
