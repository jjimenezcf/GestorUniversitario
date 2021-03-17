using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class ampliartabladepaises : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISO2",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "VARCHAR(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NAME",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "VARCHAR(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PREFIJO",
                schema: "CALLEJERO",
                table: "PAIS",
                type: "VARCHAR(10)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISO2",
                schema: "CALLEJERO",
                table: "PAIS");

            migrationBuilder.DropColumn(
                name: "NAME",
                schema: "CALLEJERO",
                table: "PAIS");

            migrationBuilder.DropColumn(
                name: "PREFIJO",
                schema: "CALLEJERO",
                table: "PAIS");
        }
    }
}
