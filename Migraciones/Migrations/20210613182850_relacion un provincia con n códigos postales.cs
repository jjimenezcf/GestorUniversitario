using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class relacionunprovinciaconncódigospostales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PROVINCIA_CP",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PPROVINCIA = table.Column<int>(type: "INT", nullable: false),
                    ID_CP = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROVINCIA_CP", x => x.ID);
                    table.UniqueConstraint("AK_PROVINCIA_CP_ID_CP", x => x.ID_CP);
                    table.ForeignKey(
                        name: "FK_PROVINCIA_CP_ID_CP",
                        column: x => x.ID_CP,
                        principalSchema: "CALLEJERO",
                        principalTable: "CODIGO_POSTAL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROVINCIA_CP_ID_PROVINCIA",
                        column: x => x.ID_PPROVINCIA,
                        principalSchema: "CALLEJERO",
                        principalTable: "PROVINCIA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_CP_ID_CP",
                schema: "CALLEJERO",
                table: "PROVINCIA_CP",
                column: "ID_CP");

            migrationBuilder.CreateIndex(
                name: "I_PROVINCIA_CP_ID_PROVINCIA",
                schema: "CALLEJERO",
                table: "PROVINCIA_CP",
                column: "ID_PPROVINCIA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROVINCIA_CP",
                schema: "CALLEJERO");
        }
    }
}
