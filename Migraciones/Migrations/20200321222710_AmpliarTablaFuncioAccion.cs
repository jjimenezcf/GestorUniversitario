using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class AmpliarTablaFuncioAccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUN_ELEMENTO",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "FUN_ACCION",
                schema: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "ACCION",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CONTROLADOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    ACCION = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    PARAMETROS = table.Column<string>(type: "VARCHAR(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FUNCION",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    ICONO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    ACTIVO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    IDPADRE = table.Column<int>(type: "INT", nullable: true),
                    IDACCION = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FUNCION_IDACCION",
                        column: x => x.IDACCION,
                        principalSchema: "ENTORNO",
                        principalTable: "ACCION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FUNCION_IDPADRE",
                        column: x => x.IDPADRE,
                        principalSchema: "ENTORNO",
                        principalTable: "FUNCION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACCION",
                schema: "ENTORNO",
                table: "ACCION",
                columns: new[] { "CONTROLADOR", "ACCION", "PARAMETROS" },
                unique: true,
                filter: "[PARAMETROS] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FUNCION_IDACCION",
                schema: "ENTORNO",
                table: "FUNCION",
                column: "IDACCION");

            migrationBuilder.CreateIndex(
                name: "IX_FUNCION_IDPADRE",
                schema: "ENTORNO",
                table: "FUNCION",
                column: "IDPADRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUNCION",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "ACCION",
                schema: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "FUN_ACCION",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACCION = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CONTROLADOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    PARAMETROS = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUN_ACCION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FUN_ELEMENTO",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACTIVO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    IDACCION = table.Column<int>(type: "INT", nullable: true),
                    IDPADRE = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
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
    }
}
