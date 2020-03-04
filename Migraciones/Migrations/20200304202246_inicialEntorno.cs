using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class inicialEntorno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "FUN_ACCION",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONTROLADOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    ACCION = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    PARAMETROS = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUN_ACCION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VAR_ELEMENTO",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    VALOR = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAR_ELEMENTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FUN_ELEMENTO",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPADRE = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    ACTIVO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    IDACCION = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUN_ELEMENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FUN_ELEMENTO_FUN_ACCION_IDACCION",
                        column: x => x.IDACCION,
                        principalSchema: "ENTORNO",
                        principalTable: "FUN_ACCION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FUN_ELEMENTO_IDACCION",
                schema: "ENTORNO",
                table: "FUN_ELEMENTO",
                column: "IDACCION");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUN_ELEMENTO",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "VAR_ELEMENTO",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "FUN_ACCION",
                schema: "ENTORNO");
        }
    }
}
