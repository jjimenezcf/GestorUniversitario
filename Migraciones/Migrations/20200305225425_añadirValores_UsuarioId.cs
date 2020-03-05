using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class añadirValores_UsuarioId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update [UNIVERSIDAD].[EST_CURSO] set UsuarioId = EstudianteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update [UNIVERSIDAD].[EST_CURSO] set UsuarioId = NULL");
        }
    }
}
