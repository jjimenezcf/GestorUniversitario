using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificarlafechadetrazaylogdeuntrabajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "INICIADO",
                schema: "TRABAJO",
                table: "SEMAFORO",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECHA",
                schema: "TRABAJO",
                table: "ERROR",
                type: "DATETIME2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "INICIADO",
                schema: "TRABAJO",
                table: "SEMAFORO",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FECHA",
                schema: "TRABAJO",
                table: "ERROR",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(7)");
        }
    }
}
