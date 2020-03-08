using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CREAR_tablas_De_Puesto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropColumn(
                name: "Descri",
                schema: "SEGURIDAD",
                table: "ROL");

            migrationBuilder.AddColumn<string>(
                name: "DESCRIPCION",
                schema: "SEGURIDAD",
                table: "ROL",
                type: "VARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ROL_PERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                columns: new[] { "IDROL", "IDPERMISO" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PERMISO_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                columns: new[] { "CLASE", "PERMISO" });

            migrationBuilder.CreateTable(
                name: "PUESTO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    DESCRIPCION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUESTO", x => x.ID);
                    table.UniqueConstraint("AK_PUESTO_NOMBRE", x => x.NOMBRE);
                });

            migrationBuilder.CreateTable(
                name: "ROL_PUESTO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDROL = table.Column<int>(type: "INT", nullable: false),
                    IDPUESTO = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL_PUESTO", x => x.ID);
                    table.UniqueConstraint("AK_ROL_PUESTO", x => new { x.IDROL, x.IDPUESTO });
                    table.ForeignKey(
                        name: "FK_ROL_PUESTO_IDROL",
                        column: x => x.IDROL,
                        principalSchema: "SEGURIDAD",
                        principalTable: "ROL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ROL_PUESTO_IDPUESTO",
                        column: x => x.IDPUESTO,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PUESTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "ROL_PUESTO",
                column: "IDPUESTO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROL_PUESTO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "PUESTO",
                schema: "SEGURIDAD");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ROL_PERMISO",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PERMISO_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "DESCRIPCION",
                schema: "SEGURIDAD",
                table: "ROL");

            migrationBuilder.AddColumn<string>(
                name: "Descri",
                schema: "SEGURIDAD",
                table: "ROL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROL_PERMISO_IDROL",
                schema: "SEGURIDAD",
                table: "ROL_PERMISO",
                column: "IDROL");
        }
    }
}
