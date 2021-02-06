using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class añadirtrabajossometipos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TRABAJO");

            migrationBuilder.CreateTable(
                name: "TRABAJO",
                schema: "TRABAJO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ES_DLL = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    DLL = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    CLASE = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    METODO = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    ESQUEMA = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    PA = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    COMUNICAR_FIN = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    COMUNICAR_ERROR = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    ID_EJECUTOR = table.Column<int>(type: "INT", nullable: true),
                    ID_INFORMAR_A = table.Column<int>(type: "INT", nullable: true),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRABAJO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USUARIO_IDEJECUTOR",
                        column: x => x.ID_EJECUTOR,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USUARIO_IDPUESTO",
                        column: x => x.ID_INFORMAR_A,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PUESTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_ID_EJECUTOR",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_EJECUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_ID_INFORMAR_A",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "ID_INFORMAR_A");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_METODO",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "DLL", "CLASE", "METODO" },
                unique: true,
                filter: "[DLL] IS NOT NULL AND [CLASE] IS NOT NULL AND [METODO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_NOMBRE",
                schema: "TRABAJO",
                table: "TRABAJO",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRABAJO_PA",
                schema: "TRABAJO",
                table: "TRABAJO",
                columns: new[] { "ESQUEMA", "PA" },
                unique: true,
                filter: "[ESQUEMA] IS NOT NULL AND [PA] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRABAJO",
                schema: "TRABAJO");
        }
    }
}
