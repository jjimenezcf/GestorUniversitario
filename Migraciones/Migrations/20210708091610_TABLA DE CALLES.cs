using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class TABLADECALLES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            CREATE FUNCTION [CALLEJERO].[CC_CALLE_EXPRESION] (@NOMBRE VarChar(250), @ID_MUNICIPIO int, @ID_TIPO_DE_VIA int)
            RETURNS VarChar(2000)
            AS
            begin
              declare @municipio varchar(250)
              declare @tipoVia varchar(250)
              declare @resultado VARCHAR(250)
              
              select @municipio = NOMBRE from callejero.MUNICIPIO where id = @ID_MUNICIPIO
              select @tipoVia = NOMBRE from callejero.TIPO_VIA id where id = @ID_TIPO_DE_VIA
              
              select @resultado = @tipoVia + ' ' + @NOMBRE + '(' + @municipio + ')'
              
              return @resultado
            END");

            migrationBuilder.CreateTable(
                name: "CALLE",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODIGO = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    ID_MUNICIPIO = table.Column<int>(type: "INT", nullable: false),
                    ID_TIPO_DE_VIA = table.Column<int>(type: "INT", nullable: false),
                    EXPRESION = table.Column<string>(type: "VARCHAR(MAX)", nullable: true, computedColumnSql: "CALLEJERO.CC_CALLE_EXPRESION(NOMBRE, ID_MUNICIPIO, ID_TIPO_DE_VIA)"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    FECCRE = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false),
                    IDUSUCREA = table.Column<int>(type: "INT", nullable: false),
                    FECMOD = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true),
                    IDUSUMODI = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALLE", x => x.ID);
                    table.UniqueConstraint("AK_CALLE_ID_MUNICIPIO_CODIGO", x => new { x.ID_MUNICIPIO, x.CODIGO });
                    table.ForeignKey(
                        name: "FK_CALLE_ID_MUNICIPIO",
                        column: x => x.ID_MUNICIPIO,
                        principalSchema: "CALLEJERO",
                        principalTable: "MUNICIPIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CALLE_ID_TIPO_DE_VIA",
                        column: x => x.ID_TIPO_DE_VIA,
                        principalSchema: "CALLEJERO",
                        principalTable: "TIPO_VIA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CALLE_IDUSUCREA",
                        column: x => x.IDUSUCREA,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CALLE_IDUSUMODI",
                        column: x => x.IDUSUMODI,
                        principalSchema: "ENTORNO",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CALLE_AUDITORIA",
                schema: "CALLEJERO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_ELEMENTO = table.Column<int>(type: "INT", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "INT", nullable: false),
                    OPERACION = table.Column<string>(type: "CHAR(1)", nullable: false),
                    REGISTRO = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    AUDITADO_EL = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALLE_AUDITORIA", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "I_CALLE_ID_MUNICIPIO",
                schema: "CALLEJERO",
                table: "CALLE",
                column: "ID_MUNICIPIO");

            migrationBuilder.CreateIndex(
                name: "I_CALLE_IDUSUCREA",
                schema: "CALLEJERO",
                table: "CALLE",
                column: "IDUSUCREA");

            migrationBuilder.CreateIndex(
                name: "I_CALLE_IDUSUMODI",
                schema: "CALLEJERO",
                table: "CALLE",
                column: "IDUSUMODI");

            migrationBuilder.CreateIndex(
                name: "I_CALLE_NOMBRE",
                schema: "CALLEJERO",
                table: "CALLE",
                column: "NOMBRE");

            migrationBuilder.CreateIndex(
                name: "IX_CALLE_ID_TIPO_DE_VIA",
                schema: "CALLEJERO",
                table: "CALLE",
                column: "ID_TIPO_DE_VIA");

            migrationBuilder.CreateIndex(
                name: "I_CALLE_AUDITORIA_ID_ELEMENTO",
                schema: "CALLEJERO",
                table: "CALLE_AUDITORIA",
                column: "ID_ELEMENTO");

            migrationBuilder.CreateIndex(
                name: "I_CALLE_AUDITORIA_ID_USUARIO",
                schema: "CALLEJERO",
                table: "CALLE_AUDITORIA",
                column: "ID_USUARIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CALLE",
                schema: "CALLEJERO");

            migrationBuilder.DropTable(
                name: "CALLE_AUDITORIA",
                schema: "CALLEJERO");
        }
    }
}
