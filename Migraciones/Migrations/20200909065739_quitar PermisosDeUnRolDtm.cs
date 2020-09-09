using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class quitarPermisosDeUnRolDtm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "PERMISO_ROL",
            //    schema: "SEGURIDAD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "PERMISO_ROL",
            //    schema: "SEGURIDAD",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "INT", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IDPERMISO = table.Column<int>(type: "INT", nullable: false),
            //        IDROL = table.Column<int>(type: "INT", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PERMISO_ROL", x => x.ID);
            //        table.UniqueConstraint("AK_ROL_PERMISO", x => new { x.IDROL, x.IDPERMISO });
            //        table.ForeignKey(
            //            name: "FK_ROL_PERMISO_IDPERMISO",
            //            column: x => x.IDPERMISO,
            //            principalSchema: "SEGURIDAD",
            //            principalTable: "PERMISO",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ROL_PERMISO_IDROL",
            //            column: x => x.IDROL,
            //            principalSchema: "SEGURIDAD",
            //            principalTable: "ROL",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PERMISO_ROL_IDPERMISO",
            //    schema: "SEGURIDAD",
            //    table: "PERMISO_ROL",
            //    column: "IDPERMISO");
        }
    }
}
