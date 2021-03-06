using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class retoquesentrabajoerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ERROR",
                schema: "TRABAJO",
                table: "ERROR",
                type: "VARCHAR(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)");

            migrationBuilder.AddColumn<string>(
                name: "DETALLE",
                schema: "TRABAJO",
                table: "ERROR",
                type: "VARCHAR(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DETALLE",
                schema: "TRABAJO",
                table: "ERROR");

            migrationBuilder.AlterColumn<string>(
                name: "ERROR",
                schema: "TRABAJO",
                table: "ERROR",
                type: "VARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)");
        }
    }
}
