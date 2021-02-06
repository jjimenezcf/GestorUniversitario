using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirtabladelog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG",
                schema: "TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TRABAJO = table.Column<int>(type: "INT", nullable: false),
                    LOG = table.Column<string>(type: "VARCHAR(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LOG_DE_TRABAJO_ID_TRABAJO",
                        column: x => x.ID_TRABAJO,
                        principalSchema: "TRABAJO",
                        principalTable: "TRABAJO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOG_ID_TRABAJO",
                schema: "TRABAJO",
                table: "LOG",
                column: "ID_TRABAJO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOG",
                schema: "TRABAJO");
        }
    }
}
