using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class añadir_clase_permisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_PERMISO_PERMISO",
            //    schema: "SEGURIDAD",
            //    table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "CLASE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.AddColumn<int>(
                name: "IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CLASE_PERMISO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLASE_PERMISO", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "I_PERMISO_IDCLASE_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO",
                columns: new[] { "IDCLASE", "PERMISO" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "I_CLASE_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "CLASE_PERMISO",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "IDCLASE",
                principalSchema: "SEGURIDAD",
                principalTable: "CLASE_PERMISO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PERMISO_IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropTable(
                name: "CLASE_PERMISO",
                schema: "SEGURIDAD");

            migrationBuilder.DropIndex(
                name: "I_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropIndex(
                name: "I_PERMISO_IDCLASE_PERMISO",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.DropColumn(
                name: "IDCLASE",
                schema: "SEGURIDAD",
                table: "PERMISO");

            migrationBuilder.AddColumn<string>(
                name: "CLASE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                type: "VARCHAR(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "PERMISO",
                column: "NOMBRE");

            //migrationBuilder.AddUniqueConstraint(
            //    name: "AK_PERMISO_PERMISO",
            //    schema: "SEGURIDAD",
            //    table: "PERMISO",
            //    columns: new[] { "CLASE", "PERMISO" });
        }
    }
}
