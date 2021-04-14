using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificarelementodtm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                type: "DATETIME2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "DATETIME2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "SISDOC",
                table: "ARCHIVO",
                type: "DATETIME2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "SISDOC",
                table: "ARCHIVO",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "CALLEJERO",
                table: "PROVINCIA",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECMOD",
                schema: "SISDOC",
                table: "ARCHIVO",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECCRE",
                schema: "SISDOC",
                table: "ARCHIVO",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");
        }
    }
}
