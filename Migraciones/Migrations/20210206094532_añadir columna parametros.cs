using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadircolumnaparametros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PARAMETROS",
                schema: "TRABAJO",
                table: "USUARIO",
                type: "VARCHAR(2000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PARAMETROS",
                schema: "TRABAJO",
                table: "USUARIO");
        }
    }
}
