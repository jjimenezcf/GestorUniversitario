using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class DefinirPermisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EST_CURSO_USUARIO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropTable(
                name: "USUARIO",
                schema: "USUARIO");

            migrationBuilder.DropIndex(
                name: "IX_EST_CURSO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "USUARIO");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "USUARIO",
                schema: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    F_ALTA = table.Column<DateTime>(type: "DATE", nullable: false),
                    APELLIDO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    LOGIN = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EST_CURSO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_EST_CURSO_USUARIO_UsuarioId",
                schema: "UNIVERSIDAD",
                table: "EST_CURSO",
                column: "UsuarioId",
                principalSchema: "USUARIO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
