using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class ajustarnombresdefuncionyaccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUNCION",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "ACCION",
                schema: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "VISTA_MVC",
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
                    table.PrimaryKey("PK_VISTA_MVC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MENU",
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
                    IDVISTA_MVC = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MENU_IDPADRE",
                        column: x => x.IDPADRE,
                        principalSchema: "ENTORNO",
                        principalTable: "MENU",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_IDVISTA_MVC",
                        column: x => x.IDVISTA_MVC,
                        principalSchema: "ENTORNO",
                        principalTable: "VISTA_MVC",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MENU_IDPADRE",
                schema: "ENTORNO",
                table: "MENU",
                column: "IDPADRE");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_IDVISTA_MVC",
                schema: "ENTORNO",
                table: "MENU",
                column: "IDVISTA_MVC");

            migrationBuilder.CreateIndex(
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VISTA_MVC",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                columns: new[] { "CONTROLADOR", "ACCION", "PARAMETROS" },
                unique: true,
                filter: "[PARAMETROS] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MENU",
                schema: "ENTORNO");

            migrationBuilder.DropTable(
                name: "VISTA_MVC",
                schema: "ENTORNO");

            migrationBuilder.CreateTable(
                name: "ACCION",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACCION = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    CONTROLADOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
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
                    ACTIVO = table.Column<string>(type: "CHAR(1)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    ICONO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    IDACCION = table.Column<int>(type: "INT", nullable: true),
                    IDPADRE = table.Column<int>(type: "INT", nullable: true),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
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
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "ACCION",
                column: "NOMBRE",
                unique: true);

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
    }
}
