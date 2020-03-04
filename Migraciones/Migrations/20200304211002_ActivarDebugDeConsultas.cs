using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class ActivarDebugDeConsultas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [ENTORNO].[VAR_ELEMENTO]
                                               ([NOMBRE]
                                               ,[DESCRIPCION]
                                               ,[VALOR])
                                         VALUES
                                               ('DebugarSqls'
                                               ,'Indica si se han de generar una traza con las sentencias SQL ejecutadas'
                                               ,'S'
                                               )
                                  ");
                                    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE 
                                   FROM [ENTORNO].[VAR_ELEMENTO]
                                   WHERE [NOMBRE] LIKE 'DebugarSqls'");

        }
    }
}
