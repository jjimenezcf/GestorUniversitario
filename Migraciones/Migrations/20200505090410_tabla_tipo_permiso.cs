using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class tabla_tipo_permiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TIPO_PERMISO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_PERMISO", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "I_TIPO_PERMISO_NOMBRE",
                schema: "SEGURIDAD",
                table: "TIPO_PERMISO",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.Sql($@" insert into seguridad.tipo_permiso(nombre) values('Gestor')
                                     go
                                     insert into seguridad.tipo_permiso(nombre) values('Consultor')
                                     go
                                     insert into seguridad.tipo_permiso(nombre) values('Administrador')
                                     go
                                     ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TIPO_PERMISO",
                schema: "SEGURIDAD");
        }
    }
}
