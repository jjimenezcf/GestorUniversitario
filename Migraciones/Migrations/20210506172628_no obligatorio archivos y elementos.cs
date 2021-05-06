using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class noobligatorioarchivosyelementos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ELEMENTOS",
                schema: "TRABAJO",
                table: "CORREO",
                type: "VARCHAR(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVOS",
                schema: "TRABAJO",
                table: "CORREO",
                type: "VARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ELEMENTOS",
                schema: "TRABAJO",
                table: "CORREO",
                type: "VARCHAR(2000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ARCHIVOS",
                schema: "TRABAJO",
                table: "CORREO",
                type: "VARCHAR(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
