using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class crearlatablapais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CALLEJERO");

            migrationBuilder.CreateTable(
                name: "PAIS",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODIGO = table.Column<string>(type: "VARCHAR(3)", nullable: false),
                    FECCRE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IDUSUCREA = table.Column<int>(type: "INT", nullable: false),
                    FECMOD = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IDUSUMODI = table.Column<int>(type: "INT", nullable: true),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAIS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PAIS_IDUSUCREA",
                        column: x => x.IDUSUCREA,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PAIS_IDUSUMODI",
                        column: x => x.IDUSUMODI,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "I_PAIS_IDUSUCREA",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "IDUSUCREA");

            migrationBuilder.CreateIndex(
                name: "I_PAIS_IDUSUMODI",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "IDUSUMODI");

            migrationBuilder.CreateIndex(
                name: "I_PAIS_NOMBRE",
                schema: "CALLEJERO",
                table: "PAIS",
                column: "NOMBRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PAIS",
                schema: "CALLEJERO");
        }
    }
}
