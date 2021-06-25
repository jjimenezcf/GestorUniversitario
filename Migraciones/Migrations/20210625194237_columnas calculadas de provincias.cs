using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class columnascalculadasdeprovincias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE FUNCTION [CALLEJERO].[OBTENER_PROVINCIA] (@CP CHAR(5))
RETURNS VarChar(250)
AS
begin
  declare @resultado VARCHAR(250)
  select @resultado = NOMBRE FROM CALLEJERO.PROVINCIA WHERE CODIGO = SUBSTRING(@CP,1,2)
  return @resultado
END
");

            migrationBuilder.AddColumn<string>(
                name: "PROVINCIA",
                schema: "CALLEJERO",
                table: "CODIGO_POSTAL",
                type: "VARCHAR(250)",
                nullable: true,
                computedColumnSql: "CALLEJERO.OBTENER_PROVINCIA(CP)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PROVINCIA",
                schema: "CALLEJERO",
                table: "CODIGO_POSTAL");
        }
    }
}
