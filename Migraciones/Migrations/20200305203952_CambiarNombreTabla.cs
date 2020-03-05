using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CambiarNombreTabla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "NOMBRE");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "APELLIDO");

            migrationBuilder.RenameColumn(
                name: "InscritoEl",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "F_INSCRIPCION");

            migrationBuilder.AlterColumn<string>(
                name: "NOMBRE",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "VARCHAR(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APELLIDO",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "VARCHAR(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "F_INSCRIPCION",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NOMBRE",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "APELLIDO",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "Apellido");

            migrationBuilder.RenameColumn(
                name: "F_INSCRIPCION",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                newName: "InscritoEl");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(250)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InscritoEl",
                schema: "UNIVERSIDAD",
                table: "EST_ELEMENTO",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");
        }
    }
}
