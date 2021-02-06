using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirtrabajosdeusuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USUARIO_IDEJECUTOR",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.DropForeignKey(
                name: "FK_USUARIO_IDPUESTO",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.CreateTable(
                name: "USUARIO",
                schema: "TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TRABAJO = table.Column<int>(type: "INT", nullable: false),
                    ID_EJECUTOR = table.Column<int>(type: "INT", nullable: false),
                    ID_SOMETEDOR = table.Column<int>(type: "INT", nullable: false),
                    ENTRADA = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    PLANIFICADO = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    INICIADO = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    TERMINADO = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ESTADO = table.Column<string>(type: "CHAR(2)", nullable: false),
                    PERIODICIDAD = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TRABAJO_DE_USUARIO_ID_EJECUTOR",
                        column: x => x.ID_EJECUTOR,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRABAJO_DE_USUARIO_ID_SOMETEDOR",
                        column: x => x.ID_SOMETEDOR,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRABAJO_DE_USUARIO_ID_TRABAJO",
                        column: x => x.ID_TRABAJO,
                        principalSchema: "TRABAJO",
                        principalTable: "TRABAJO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_EJECUTOR",
                schema: "TRABAJO",
                table: "USUARIO",
                column: "ID_EJECUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_SOMETEDOR",
                schema: "TRABAJO",
                table: "USUARIO",
                column: "ID_SOMETEDOR");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_TRABAJO",
                schema: "TRABAJO",
                table: "USUARIO",
                column: "ID_TRABAJO");

            migrationBuilder.AddForeignKey(
                name: "FK_TRABAJO_ID_EJECUTOR",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_EJECUTOR",
                principalSchema: "ENTORNO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TRABAJO_ID_INFORMAR_A",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_INFORMAR_A",
                principalSchema: "SEGURIDAD",
                principalTable: "PUESTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRABAJO_ID_EJECUTOR",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.DropForeignKey(
                name: "FK_TRABAJO_ID_INFORMAR_A",
                schema: "TRABAJO",
                table: "TRABAJO");

            migrationBuilder.DropTable(
                name: "USUARIO",
                schema: "TRABAJO");

            migrationBuilder.AddForeignKey(
                name: "FK_USUARIO_IDEJECUTOR",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_EJECUTOR",
                principalSchema: "ENTORNO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_USUARIO_IDPUESTO",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_INFORMAR_A",
                principalSchema: "SEGURIDAD",
                principalTable: "PUESTO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
