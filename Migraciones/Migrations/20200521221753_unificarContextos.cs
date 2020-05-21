using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class unificarContextos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "I_PUESTO_NOMBRE",
                schema: "SEGURIDAD",
                table: "PUESTO",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "I_ROL_NOMBRE",
                schema: "SEGURIDAD",
                table: "ROL",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "I_USU_PUESTO_IDUSUA",
                schema: "SEGURIDAD",
                table: "USU_PUESTO",
                column: "IDUSUA");

            migrationBuilder.CreateIndex(
                name: "I_USU_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "USU_PUESTO",
                column: "IDPUESTO");

            migrationBuilder.CreateIndex(
                name: "I_USU_PUESTO_IDPUESTO_IDUSUA",
                schema: "SEGURIDAD",
                table: "USU_PUESTO",
                columns: new[] { "IDPUESTO", "IDUSUA" },
                unique: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
