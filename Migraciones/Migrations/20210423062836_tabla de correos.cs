using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class tabladecorreos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CORREO",
                schema: "TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMISOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    RECEPTORES = table.Column<string>(type: "VARCHAR(2000)", nullable: false),
                    ASUNTO = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    CUERPO = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    ELEMENTOS = table.Column<string>(type: "VARCHAR(2000)", nullable: false),
                    ARCHIVOS = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "INT", nullable: false),
                    CREADO = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    ENVIADO = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORREO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TRABAJO_DE_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CORREO_ID_USUARIO",
                schema: "TRABAJO",
                table: "CORREO",
                column: "ID_USUARIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CORREO",
                schema: "TRABAJO");
        }
    }
}
