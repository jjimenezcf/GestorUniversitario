using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class tabladeprovincia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PROVINCIA",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODIGO = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    SIGLA = table.Column<string>(type: "VARCHAR(3)", nullable: false),
                    PREFIJO = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    ID_PAIS = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    FECCRE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IDUSUCREA = table.Column<int>(type: "INT", nullable: false),
                    FECMOD = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IDUSUMODI = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROVINCIA", x => x.ID);
                    table.UniqueConstraint("AK_PROVINCIA_CODIGO", x => x.CODIGO);
                    table.UniqueConstraint("AK_PROVINCIA_PREFIJO", x => x.PREFIJO);
                    table.UniqueConstraint("AK_PROVINCIA_SIGLA", x => x.SIGLA);
                    table.ForeignKey(
                        name: "FK_PROVINCIA_ID_PAIS",
                        column: x => x.ID_PAIS,
                        principalSchema: "CALLEJERO",
                        principalTable: "PAIS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROVINCIA_IDUSUCREA",
                        column: x => x.IDUSUCREA,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROVINCIA_IDUSUMODI",
                        column: x => x.IDUSUMODI,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_ID_PAIS",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                column: "ID_PAIS");

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_IDUSUCREA",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                column: "IDUSUCREA");

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_IDUSUMODI",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                column: "IDUSUMODI");

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_NOMBRE",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                column: "NOMBRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROVINCIA",
                schema: "CALLEJERO");
        }
    }
}
