using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class relacionunmunicipioconncódigospostales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID_PPROVINCIA",
                schema: "CALLEJERO",
                table: "PROVINCIA_CP",
                newName: "ID_PROVINCIA");

            migrationBuilder.CreateTable(
                name: "MUNICIPIO_CP",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_MUNICIPIO = table.Column<int>(type: "INT", nullable: false),
                    ID_CP = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUNICIPIO_CP", x => x.ID);
                    table.UniqueConstraint("AK_MUNICIPIO_CP_ID_CP", x => x.ID_CP);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_CP_ID_CP",
                        column: x => x.ID_CP,
                        principalSchema: "CALLEJERO",
                        principalTable: "CODIGO_POSTAL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MUNICIPIO_CP_ID_MUNICIPIO",
                        column: x => x.ID_MUNICIPIO,
                        principalSchema: "CALLEJERO",
                        principalTable: "MUNICIPIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_CP_ID_CP",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP",
                column: "ID_CP");

            migrationBuilder.CreateIndex(
                name: "I_MUNICIPIO_CP_ID_MUNICIPIO",
                schema: "CALLEJERO",
                table: "MUNICIPIO_CP",
                column: "ID_MUNICIPIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MUNICIPIO_CP",
                schema: "CALLEJERO");

            migrationBuilder.RenameColumn(
                name: "ID_PROVINCIA",
                schema: "CALLEJERO",
                table: "PROVINCIA_CP",
                newName: "ID_PPROVINCIA");
        }
    }
}
