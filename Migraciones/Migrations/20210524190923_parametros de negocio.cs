using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class parametrosdenegocio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PARAMETRO",
                schema: "NEGOCIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VALOR = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    ID_NEGOCIO = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARAMETRO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NEGOCIO_PARAMETRO_ID_NEGOCIO",
                        column: x => x.ID_NEGOCIO,
                        principalSchema: "NEGOCIO",
                        principalTable: "NEGOCIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NEGOCIO_ID_NEGOCIO_NOMBRE",
                schema: "NEGOCIO",
                table: "PARAMETRO",
                columns: new[] { "ID_NEGOCIO", "NOMBRE" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PARAMETRO",
                schema: "NEGOCIO");
        }
    }
}
