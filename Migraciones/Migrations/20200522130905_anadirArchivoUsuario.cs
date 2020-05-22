using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class anadirArchivoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "I_USUARIO_IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO",
                column: "IDARCHIVO");

            migrationBuilder.AddForeignKey(
                name: "FK_USUARIO_IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO",
                column: "IDARCHIVO",
                principalSchema: "SISDOC",
                principalTable: "ARCHIVO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USUARIO_IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO");

            migrationBuilder.DropIndex(
                name: "I_USUARIO_IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO");

            migrationBuilder.DropColumn(
                name: "IDARCHIVO",
                schema: "ENTORNO",
                table: "USUARIO");
        }
    }
}
