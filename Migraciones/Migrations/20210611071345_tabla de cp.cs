using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class tabladecp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CODIGO_POSTAL",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CP = table.Column<string>(type: "VARCHAR(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CODIGO_POSTAL", x => x.ID);
                    table.UniqueConstraint("AK_CODIGO_POSTAL_CP", x => x.CP);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CODIGO_POSTAL",
                schema: "CALLEJERO");
        }
    }
}
