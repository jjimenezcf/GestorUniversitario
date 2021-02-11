using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class verquepasaconlosbooleanos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "COMUNICAR_FIN",
                schema: "TRABAJO",
                table: "TRABAJO",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "COMUNICAR_ERROR",
                schema: "TRABAJO",
                table: "TRABAJO",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "COMUNICAR_FIN",
                schema: "TRABAJO",
                table: "TRABAJO",
                type: "BIT",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AlterColumn<bool>(
                name: "COMUNICAR_ERROR",
                schema: "TRABAJO",
                table: "TRABAJO",
                type: "BIT",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "BIT");
        }
    }
}
