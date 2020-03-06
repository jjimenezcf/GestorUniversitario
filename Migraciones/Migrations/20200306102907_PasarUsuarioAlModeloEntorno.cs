using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class PasarUsuarioAlModeloEntorno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USUARIO",
                schema: "ENTORNO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOGIN = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    APELLIDO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    F_ALTA = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USUARIO",
                schema: "ENTORNO");
        }
    }
}
