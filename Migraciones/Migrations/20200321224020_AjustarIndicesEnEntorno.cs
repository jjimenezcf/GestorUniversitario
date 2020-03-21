using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class AjustarIndicesEnEntorno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VAR_ELEMENTO",
                schema: "ENTORNO",
                table: "VAR_ELEMENTO");

            migrationBuilder.RenameTable(
                name: "VAR_ELEMENTO",
                schema: "ENTORNO",
                newName: "VARIABLE",
                newSchema: "ENTORNO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VARIABLE",
                schema: "ENTORNO",
                table: "VARIABLE",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO",
                schema: "ENTORNO",
                table: "USUARIO",
                column: "LOGIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "ACCION",
                column: "NOMBRE",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_USUARIO",
                schema: "ENTORNO",
                table: "USUARIO");

            migrationBuilder.DropIndex(
                name: "IX_VARIABLE",
                schema: "ENTORNO",
                table: "ACCION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VARIABLE",
                schema: "ENTORNO",
                table: "VARIABLE");

            migrationBuilder.RenameTable(
                name: "VARIABLE",
                schema: "ENTORNO",
                newName: "VAR_ELEMENTO",
                newSchema: "ENTORNO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VAR_ELEMENTO",
                schema: "ENTORNO",
                table: "VAR_ELEMENTO",
                column: "ID");
        }
    }
}
