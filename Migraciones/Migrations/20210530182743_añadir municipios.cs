using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirmunicipios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MUNICIPIO",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODIGO = table.Column<string>(type: "VARCHAR(3)", nullable: false),
                    DC = table.Column<string>(type: "VARCHAR(1)", nullable: false),
                    ID_PROVINCIA = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    FECCRE = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    IDUSUCREA = table.Column<int>(type: "INT", nullable: false),
                    FECMOD = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true),
                    IDUSUMODI = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUNICIPIO", x => x.ID);
                    table.UniqueConstraint("AK_MUNICIPIO_CODIGO", x => x.CODIGO);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_ID_PROVINCIA",
                        column: x => x.ID_PROVINCIA,
                        principalSchema: "CALLEJERO",
                        principalTable: "PROVINCIA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_IDUSUCREA",
                        column: x => x.IDUSUCREA,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_IDUSUMODI",
                        column: x => x.IDUSUMODI,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MUNICIPIO_AUDITORIA",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_ELEMENTO = table.Column<int>(type: "INT", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "INT", nullable: false),
                    OPERACION = table.Column<string>(type: "CHAR(1)", nullable: false),
                    REGISTRO = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    AUDITADO_EL = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUNICIPIO_AUDITORIA", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_ID_PROVINCIA",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                column: "ID_PROVINCIA");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_IDUSUCREA",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                column: "IDUSUCREA");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_IDUSUMODI",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                column: "IDUSUMODI");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_NOMBRE",
                schema: "CALLEJERO",
                table: "MUNICIPIO",
                column: "NOMBRE");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "MUNICIPIO_AUDITORIA",
                column: "ID_ELEMENTO");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "MUNICIPIO_AUDITORIA",
                column: "ID_USUARIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MUNICIPIO",
                schema: "CALLEJERO");

            migrationBuilder.DropTable(
                name: "MUNICIPIO_AUDITORIA",
                schema: "CALLEJERO");
        }
    }
}
