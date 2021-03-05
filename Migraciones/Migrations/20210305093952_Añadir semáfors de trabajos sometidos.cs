using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class Añadirsemáforsdetrabajossometidos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SEMAFORO",
                schema: "TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TRABAJO = table.Column<int>(type: "INT", nullable: false),
                    INICIADO = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    LOGIN = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEMAFORO", x => x.ID);
                    table.UniqueConstraint("AK_SEMAFORO_TRABAJO_ID_TRABAJO", x => x.ID_TRABAJO);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEMAFORO",
                schema: "TRABAJO");
        }
    }
}
