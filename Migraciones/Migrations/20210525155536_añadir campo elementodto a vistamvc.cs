using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadircampoelementodtoavistamvc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ELEMENTO_DTO",
                schema: "ENTORNO",
                table: "VISTA_MVC",
                type: "VARCHAR(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ELEMENTO_DTO",
                schema: "ENTORNO",
                table: "VISTA_MVC");
        }
    }
}
