using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirtipodevía : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TIPO_VIA",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGLA = table.Column<string>(type: "VARCHAR(4)", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_VIA", x => x.ID);
                    table.UniqueConstraint("AK_TIPO_VIA_SIGLA", x => x.SIGLA);
                });

            migrationBuilder.CreateIndex(
                name: "I_TIPO_VIA_NOMBRE",
                schema: "CALLEJERO",
                table: "TIPO_VIA",
                column: "NOMBRE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TIPO_VIA",
                schema: "CALLEJERO");
        }
    }
}
