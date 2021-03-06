using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class ajustarerroresytrazas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LOG",
                schema: "TRABAJO",
                table: "LOG");

            migrationBuilder.RenameTable(
                name: "LOG",
                schema: "TRABAJO",
                newName: "TRAZA",
                newSchema: "TRABAJO");

            migrationBuilder.RenameIndex(
                name: "IX_LOG_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "TRAZA",
                newName: "IX_TRAZA_ID_TRABAJO_USUARIO");

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA",
                schema: "TRABAJO",
                table: "ERROR",
                type: "DATETIME",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECHA",
                schema: "TRABAJO",
                table: "TRAZA",
                type: "DATETIME",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TRAZA",
                schema: "TRABAJO",
                table: "TRAZA",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TRAZA",
                schema: "TRABAJO",
                table: "TRAZA");

            migrationBuilder.DropColumn(
                name: "FECHA",
                schema: "TRABAJO",
                table: "ERROR");

            migrationBuilder.DropColumn(
                name: "FECHA",
                schema: "TRABAJO",
                table: "TRAZA");

            migrationBuilder.RenameTable(
                name: "TRAZA",
                schema: "TRABAJO",
                newName: "LOG",
                newSchema: "TRABAJO");

            migrationBuilder.RenameIndex(
                name: "IX_TRAZA_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "LOG",
                newName: "IX_LOG_ID_TRABAJO_USUARIO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LOG",
                schema: "TRABAJO",
                table: "LOG",
                column: "ID");
        }
    }
}
