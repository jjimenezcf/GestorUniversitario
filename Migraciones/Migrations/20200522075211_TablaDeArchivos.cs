using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class TablaDeArchivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SISDOC");

            migrationBuilder.CreateTable(
                name: "ARCHIVO",
                schema: "SISDOC",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    FECCRE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IDUSUCREA = table.Column<int>(type: "INT", nullable: false),
                    FECMOD = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IDUSUMODI = table.Column<int>(type: "INT", nullable: true),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    RUTA = table.Column<string>(type: "VARCHAR(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARCHIVO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ARCHIVO_IDUSUCREA",
                        column: x => x.IDUSUCREA,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ARCHIVO_IDUSUMODI",
                        column: x => x.IDUSUMODI,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "I_ARCHIVO_IDUSUCREA",
                schema: "SISDOC",
                table: "ARCHIVO",
                column: "IDUSUCREA");

            migrationBuilder.CreateIndex(
                name: "I_ARCHIVO_IDUSUMODI",
                schema: "SISDOC",
                table: "ARCHIVO",
                column: "IDUSUMODI");

            migrationBuilder.CreateIndex(
                name: "I_ARCHIVO_NOMBRE",
                schema: "SISDOC",
                table: "ARCHIVO",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARCHIVO",
                schema: "SISDOC");
        }
    }
}
