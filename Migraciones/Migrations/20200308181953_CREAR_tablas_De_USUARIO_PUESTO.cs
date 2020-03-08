using Microsoft.EntityFrameworkCore.Migrations;

namespace Migraciones.Migrations
{
    public partial class CREAR_tablas_De_USUARIO_PUESTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" create view SEGURIDAD.V_USUARIO as
                                    select
                                       ID,
                                       LOGIN,
                                       APELLIDO,
                                       NOMBRE
                                    from
                                       ENTORNO.USUARIO
                                  ");

            migrationBuilder.CreateTable(
                name: "USU_PUESTO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDUSUA = table.Column<int>(type: "INT", nullable: false),
                    IDPUESTO = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USU_PUESTO", x => x.ID);
                    table.UniqueConstraint("AK_USU_PUESTO", x => new { x.IDUSUA, x.IDPUESTO });
                    table.ForeignKey(
                        name: "FK_USU_PUESTO_IDPUESTO",
                        column: x => x.IDPUESTO,
                        principalSchema: "SEGURIDAD",
                        principalTable: "PUESTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USU_PUESTO_IDUSUA",
                schema: "SEGURIDAD",
                table: "USU_PUESTO",
                column: "IDUSUA");

            migrationBuilder.CreateIndex(
                name: "IX_USU_PUESTO_IDPUESTO",
                schema: "SEGURIDAD",
                table: "USU_PUESTO",
                column: "IDPUESTO");

            migrationBuilder.Sql(@"
                                    alter table SEGURIDAD.USU_PUESTO
                                    add constraint FK_USU_PUESTO_IDUSUA foreign key (IDUSUA)
                                    references ENTORNO.USUARIO (ID)
                                  ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"alter table SEGURIDAD.USU_PUESTO drop constraint FK_USU_PUESTO_IDUSUA");

            migrationBuilder.DropTable(
                name: "USU_PUESTO",
                schema: "SEGURIDAD");


            migrationBuilder.Sql(@"drop view SEGURIDAD.V_USUARIO");

        }
    }
}
