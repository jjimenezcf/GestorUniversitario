using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class poner_activo_como_boleano : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ACTIVO",
                schema: "ENTORNO",
                table: "MENU",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ACTIVO",
                schema: "ENTORNO",
                table: "MENU",
                type: "CHAR(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");
        }
    }
}
