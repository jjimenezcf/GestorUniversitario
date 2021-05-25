using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadircampoelementodtoanegocio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ELEMENTO",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                newName: "ELEMENTO_DTM");

            migrationBuilder.AddColumn<string>(
                name: "ELEMENTO_DTO",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                type: "VARCHAR(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ELEMENTO_DTO",
                schema: "NEGOCIO",
                table: "NEGOCIO");

            migrationBuilder.RenameColumn(
                name: "ELEMENTO_DTM",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                newName: "ELEMENTO");
        }
    }
}
