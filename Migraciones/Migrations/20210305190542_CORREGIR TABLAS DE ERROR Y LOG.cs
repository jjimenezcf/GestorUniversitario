using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class CORREGIRTABLASDEERRORYLOG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ERROR_DE_TRABAJO_ID_TRABAJO",
                schema: "TRABAJO",
                table: "ERROR");

            migrationBuilder.DropForeignKey(
                name: "FK_LOG_DE_TRABAJO_ID_TRABAJO",
                schema: "TRABAJO",
                table: "LOG");

            migrationBuilder.RenameColumn(
                name: "ID_TRABAJO",
                schema: "TRABAJO",
                table: "LOG",
                newName: "ID_TRABAJO_USUARIO");

            migrationBuilder.RenameIndex(
                name: "IX_LOG_ID_TRABAJO",
                schema: "TRABAJO",
                table: "LOG",
                newName: "IX_LOG_ID_TRABAJO_USUARIO");

            migrationBuilder.RenameColumn(
                name: "ID_TRABAJO",
                schema: "TRABAJO",
                table: "ERROR",
                newName: "ID_TRABAJO_USUARIO");

            migrationBuilder.RenameIndex(
                name: "IX_ERROR_ID_TRABAJO",
                schema: "TRABAJO",
                table: "ERROR",
                newName: "IX_ERROR_ID_TRABAJO_USUARIO");

            migrationBuilder.AddForeignKey(
                name: "FK_ERROR_DE_TRABAJO_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "ERROR",
                column: "ID_TRABAJO_USUARIO",
                principalSchema: "TRABAJO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LOG_DE_TRABAJO_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "LOG",
                column: "ID_TRABAJO_USUARIO",
                principalSchema: "TRABAJO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ERROR_DE_TRABAJO_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "ERROR");

            migrationBuilder.DropForeignKey(
                name: "FK_LOG_DE_TRABAJO_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "LOG");

            migrationBuilder.RenameColumn(
                name: "ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "LOG",
                newName: "ID_TRABAJO");

            migrationBuilder.RenameIndex(
                name: "IX_LOG_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "LOG",
                newName: "IX_LOG_ID_TRABAJO");

            migrationBuilder.RenameColumn(
                name: "ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "ERROR",
                newName: "ID_TRABAJO");

            migrationBuilder.RenameIndex(
                name: "IX_ERROR_ID_TRABAJO_USUARIO",
                schema: "TRABAJO",
                table: "ERROR",
                newName: "IX_ERROR_ID_TRABAJO");

            migrationBuilder.AddForeignKey(
                name: "FK_ERROR_DE_TRABAJO_ID_TRABAJO",
                schema: "TRABAJO",
                table: "ERROR",
                column: "ID_TRABAJO",
                principalSchema: "TRABAJO",
                principalTable: "TRABAJO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LOG_DE_TRABAJO_ID_TRABAJO",
                schema: "TRABAJO",
                table: "LOG",
                column: "ID_TRABAJO",
                principalSchema: "TRABAJO",
                principalTable: "TRABAJO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
