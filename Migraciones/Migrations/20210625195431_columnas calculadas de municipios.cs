using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class columnascalculadasdemunicipios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE FUNCTION [CALLEJERO].[OBTENER_MUNICIPIOS] (@CP CHAR(5))
RETURNS VarChar(250)
AS
begin
  declare @municipio VARCHAR(250)
  declare @resultado VARCHAR(250)
  select @resultado = NOMBRE FROM CALLEJERO.MUNICIPIO WHERE CODIGO = @CP

   DECLARE c CURSOR FOR SELECT t1.NOMBRE
     from CALLEJERO.MUNICIPIO t1
	 inner join CALLEJERO.MUNICIPIO_CP t2 on t2.ID_MUNICIPIO = t1.ID
	 inner join CALLEJERO.CODIGO_POSTAL t3 on t2.ID_CP = t3.ID
	 where t3.CP = @CP
   set @resultado = ''
   OPEN c
   FETCH NEXT FROM c INTO @municipio
   WHILE @@fetch_status = 0
   BEGIN
	if @resultado = ''  
      set  @resultado = @municipio 
	else 
      set  @resultado = @resultado + ' - ' + @municipio 
    FETCH NEXT FROM c INTO @municipio
   END
   CLOSE c
   DEALLOCATE c
  return @resultado
END
");

            migrationBuilder.AddColumn<string>(
                name: "MUNICIPIOS",
                schema: "CALLEJERO",
                table: "CODIGO_POSTAL",
                type: "VARCHAR(250)",
                nullable: true,
                computedColumnSql: "CALLEJERO.OBTENER_MUNICIPIOS(CP)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MUNICIPIOS",
                schema: "CALLEJERO",
                table: "CODIGO_POSTAL");
        }
    }
}
