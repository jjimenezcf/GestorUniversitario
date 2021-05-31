using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class ampliarTablaDeNegocios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ENUMERADO",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                type: "VARCHAR(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ES_DE_PARAMETRIZACION",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "USA_SEGURIDAD",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                type: "BIT",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ENUMERADO",
                schema: "NEGOCIO",
                table: "NEGOCIO");

            migrationBuilder.DropColumn(
                name: "ES_DE_PARAMETRIZACION",
                schema: "NEGOCIO",
                table: "NEGOCIO");

            migrationBuilder.DropColumn(
                name: "USA_SEGURIDAD",
                schema: "NEGOCIO",
                table: "NEGOCIO");
        }
    }
}
