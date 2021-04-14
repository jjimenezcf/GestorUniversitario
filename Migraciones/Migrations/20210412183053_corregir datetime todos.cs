using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class corregirdatetimetodos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TERMINADO",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PLANIFICADO",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ENTRADA",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TERMINADO",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PLANIFICADO",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ENTRADA",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");
        }
    }
}
