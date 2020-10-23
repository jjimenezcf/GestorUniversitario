using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PASSWORD",
                schema: "ENTORNO",
                table: "USUARIO",
                type: "VARCHAR(250)",
                nullable: true);

            migrationBuilder.Sql("update entorno.usuario set password = ENCRYPTBYPASSPHRASE('sistemaSe', '12345678')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PASSWORD",
                schema: "ENTORNO",
                table: "USUARIO");
        }
    }
}
