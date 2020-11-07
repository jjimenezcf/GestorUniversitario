using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadircampoeMailausuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                schema: "ENTORNO",
                table: "USUARIO",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "pendiente@se.com");

            migrationBuilder.Sql($@"update entorno.usuario set email = nombre");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMAIL",
                schema: "ENTORNO",
                table: "USUARIO");
         }
    }
}
