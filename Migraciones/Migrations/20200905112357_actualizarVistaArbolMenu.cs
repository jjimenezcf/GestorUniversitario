﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class actualizarVistaArbolMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@$"DROP VIEW [ENTORNO].[MENU_SE]");
            migrationBuilder.Sql(@$"DROP FUNCTION [ENTORNO].[ARBOL_MENU]");
            migrationBuilder.Sql(@$"
CREATE FUNCTION [ENTORNO].[ARBOL_MENU] 
(
  @ID INT	
)
RETURNS TABLE 
AS
RETURN 
(
     WITH menuPadre 
     AS
     (
      SELECT T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN
      FROM entorno.MENU T1
      UNION ALL
       --RECURSIVIDAD
      SELECT S1.ID, S1.NOMBRE, S1.DESCRIPCION, S1.ICONO, S1.ACTIVO, S1.IDPADRE,  S1.IDVISTA_MVC, S1.ORDEN
      FROM ENTORNO.MENU AS S1 
      INNER JOIN menuPadre AS T1 ON S1.IDPADRE = T1.id
     )
     SELECT DISTINCT P1.ID, P1.NOMBRE, P1.DESCRIPCION, P1.ICONO, P1.ACTIVO, P1.IDPADRE, P1.IDVISTA_MVC, P1.ORDEN
     FROM menuPadre AS P1
)
");
            migrationBuilder.Sql(@$"
CREATE VIEW [ENTORNO].[MENU_SE]
AS
     SELECT T1.ID, 
            T1.NOMBRE, 
            T1.DESCRIPCION, 
            T1.ICONO, 
            T1.ACTIVO, 
            T1.IDPADRE, 
            T1.IDVISTA_MVC, 
            T1.ORDEN, 
            T2.NOMBRE AS PADRE, 
            T3.NOMBRE AS VISTA, 
            T3.CONTROLADOR, 
            T3.ACCION, 
            T3.PARAMETROS
     FROM ENTORNO.ARBOL_MENU(0) AS T1
     LEFT JOIN ENTORNO.MENU T2 ON T2.ID = T1.IDPADRE
     LEFT JOIN ENTORNO.VISTA_MVC T3 ON T3.ID = T1.IDVISTA_MVC
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"DROP VIEW [ENTORNO].[MENU_SE]");
            migrationBuilder.Sql(@$"DROP FUNCTION [ENTORNO].[ARBOL_MENU]");
            migrationBuilder.Sql(@$"
CREATE FUNCTION [ENTORNO].[ARBOL_MENU] 
(
  @ID INT	
)
RETURNS TABLE 
AS
RETURN 
(
     WITH menuPadre 
     AS
     (
      SELECT T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN, T1.IDPERMISO
      FROM entorno.MENU T1
      UNION ALL
       --RECURSIVIDAD
      SELECT S1.ID, S1.NOMBRE, S1.DESCRIPCION, S1.ICONO, S1.ACTIVO, S1.IDPADRE,  S1.IDVISTA_MVC, S1.ORDEN , S1.IDPERMISO
      FROM ENTORNO.MENU AS S1 
      INNER JOIN menuPadre AS T1 ON S1.IDPADRE = T1.id
     )
     SELECT DISTINCT P1.ID, P1.NOMBRE, P1.DESCRIPCION, P1.ICONO, P1.ACTIVO, P1.IDPADRE, P1.IDVISTA_MVC, P1.ORDEN, P1.IDPERMISO
     FROM menuPadre AS P1
)
");
             migrationBuilder.Sql(@$"

 CREATE VIEW [ENTORNO].[MENU_SE]
 AS
      SELECT T1.ID, 
             T1.NOMBRE, 
             T1.DESCRIPCION, 
             T1.ICONO, 
             T1.ACTIVO, 
             T1.IDPADRE, 
             T1.IDVISTA_MVC, 
             T1.ORDEN, 
             T1.IDPERMISO,
             T2.NOMBRE AS PADRE, 
             T3.NOMBRE AS VISTA, 
             T3.CONTROLADOR, 
             T3.ACCION, 
             T3.PARAMETROS
      FROM ENTORNO.ARBOL_MENU(0) AS T1
           LEFT JOIN ENTORNO.MENU T2 ON T2.ID = T1.IDPADRE
           LEFT JOIN ENTORNO.VISTA_MVC T3 ON T3.ID = T1.IDVISTA_MVC

");
        }
    }
}
