using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadircampoadministradorausuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ADMINISTRADOR",
                schema: "ENTORNO",
                table: "USUARIO",
                type: "BIT",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ADMINISTRADOR",
                schema: "ENTORNO",
                table: "USUARIO");
        }
    }
}
