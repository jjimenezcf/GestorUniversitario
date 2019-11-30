using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestor.Elementos.Universitario.Migrations
{
    public partial class AnadirVista : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogoDeBd");

            migrationBuilder.Sql(@"create view dbo.CatalogoDelSe as
                                   SELECT convert(int, ROW_NUMBER() OVER(ORDER BY Table_Name ASC)) as Id
                                        , TABLE_CATALOG as Catalogo
                                        , TABLE_SCHEMA as Esquema
                                        , TABLE_NAME as Tabla 
                                  FROM information_schema.tables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view  dbo.CatalogoDelSe");
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
