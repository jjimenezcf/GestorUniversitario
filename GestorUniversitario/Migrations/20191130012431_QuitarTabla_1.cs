using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestor.Elementos.Universitario.Migrations
{
    public partial class QuitarTabla_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogoDeBd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogoDeBd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Catalogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Esquema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tabla = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoDeBd", x => x.Id);
                });
        }
    }
}
