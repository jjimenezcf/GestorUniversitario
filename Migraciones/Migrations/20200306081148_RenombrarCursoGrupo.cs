using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class RenombrarCursoGrupo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_CUR_ELEMENTO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropTable(
                name: "CUR_ELEMENTO",
                schema: "UNIVERSIDAD");

            migrationBuilder.CreateTable(
                name: "GRUPO",
                schema: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CLASE = table.Column<int>(type: "INT", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_GRUPO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropTable(
                name: "GRUPO",
                schema: "USUARIO");

            migrationBuilder.CreateTable(
                name: "CUR_ELEMENTO",
                schema: "UNIVERSIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CREDITOS = table.Column<int>(type: "INT", nullable: false),
                    TITULO = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUR_ELEMENTO", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_EST_CURSO_CUR_ELEMENTO_CursoId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "CursoId",
                principalSchema: "UNIVERSIDAD",
                principalTable: "CUR_ELEMENTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
