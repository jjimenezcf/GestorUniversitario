using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestor.Elementos.Universitario.Migrations
{
    public partial class AnadirTablaDEVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Var_Variable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    Descri = table.Column<string>(maxLength: 250, nullable: true),
                    Valor = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Var_Variable", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Var_Variable");
        }
    }
}
