using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificarlafechadeauditoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AUDITADO_EL",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AUDITADO_EL",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AUDITADO_EL",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AUDITADO_EL",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");
        }
    }
}
