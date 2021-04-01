using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class Reajustarcamposdeauditoría : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PAIS_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA");

            migrationBuilder.DropForeignKey(
                name: "FK_PAIS_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA");

            migrationBuilder.DropForeignKey(
                name: "FK_PROVINCIA_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA");

            migrationBuilder.DropForeignKey(
                name: "FK_PROVINCIA_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_PAIS_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA",
                column: "ID_ELEMENTO",
                principalSchema: "CALLEJERO",
                principalTable: "PAIS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PAIS_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "PAIS_AUDITORIA",
                column: "ID_USUARIO",
                principalSchema: "ENTORNO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROVINCIA_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA",
                column: "ID_ELEMENTO",
                principalSchema: "CALLEJERO",
                principalTable: "PROVINCIA",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROVINCIA_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "PROVINCIA_AUDITORIA",
                column: "ID_USUARIO",
                principalSchema: "ENTORNO",
                principalTable: "USUARIO",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
