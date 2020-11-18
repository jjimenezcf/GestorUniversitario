using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class Añadirtabladenegociodeelementos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NEGOCIO");

            migrationBuilder.CreateTable(
                name: "NEGOCIO",
                schema: "NEGOCIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ELEMENTO = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    ICONO = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    ACTIVO = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IDPERMISO_GESTOR = table.Column<int>(type: "INT", nullable: false),
                    IDPERMISO_CONSULTOR = table.Column<int>(type: "INT", nullable: false),
                    IDPERMISO_ADMINISTRADOR = table.Column<int>(type: "INT", nullable: false),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NEGOCIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NEGOCIO_IDPERMISO_ADMINISTRADOR",
                        column: x => x.IDPERMISO_ADMINISTRADOR,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PERMISO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NEGOCIO_IDPERMISO_CONSULTOR",
                        column: x => x.IDPERMISO_CONSULTOR,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PERMISO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NEGOCIO_IDPERMISO_GESTOR",
                        column: x => x.IDPERMISO_GESTOR,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PERMISO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NEGOCIO_IDPERMISO_ADMINISTRADOR",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                column: "IDPERMISO_ADMINISTRADOR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NEGOCIO_IDPERMISO_CONSULTOR",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                column: "IDPERMISO_CONSULTOR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NEGOCIO_IDPERMISO_GESTOR",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                column: "IDPERMISO_GESTOR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NEGOCIO_NOMBRE",
                schema: "NEGOCIO",
                table: "NEGOCIO",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NEGOCIO",
                schema: "NEGOCIO");
        }
    }
}
