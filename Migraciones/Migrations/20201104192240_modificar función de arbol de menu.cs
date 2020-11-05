using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class modificarfuncióndearboldemenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
drop FUNCTION [ENTORNO].[ARBOL_MENU_POR_USUARIO] 
go

drop FUNCTION [ENTORNO].[ARBOL_MENU] 
go

create FUNCTION [ENTORNO].[ARBOL_MENU] 
(
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
go

create FUNCTION [ENTORNO].[ARBOL_MENU_POR_USUARIO] (@IDUSUARIO INT)
RETURNS 
  @MenusAccedidos TABLE 
   (ID			int 
   ,NOMBRE		varchar(250)
   ,DESCRIPCION	varchar(2000)
   ,ICONO		varchar(250)
   ,ACTIVO		bit
   ,IDPADRE		int
   ,IDVISTA_MVC int
   ,ORDEN		int)
AS
BEGIN
  declare @idpadre int
  declare @administrador bit

  select @administrador = ADMINISTRADOR from entorno.USUARIO where id = @IDUSUARIO 
                
  if 1 = @administrador  
  begin		                  
     insert @MenusAccedidos  select * from entorno.arbol_menu()
  end
  else	
  begin                  
    insert @MenusAccedidos
    SELECT T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN  
    from ENTORNO.MENU t1
    inner join ENTORNO.VISTA_MVC t2 on t2.id = t1.IDVISTA_MVC
    where t1.IDVISTA_MVC is not null
      and t1.ACTIVO = 1
      and exists (select 1 from ENTORNO.USU_PERMISO where IDUSUA = @IDUSUARIO and IDPERMISO = t2.IDPERMISO)                        
  
    declare menus  CURSOR for select IDPADRE from @MenusAccedidos 
       
    open menus
    FETCH menus INTO @idpadre
    WHILE (@@fetch_status = 0) 
    BEGIN
     insert into @MenusAccedidos  
     select T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN
     from ENTORNO.MENU t1
     where id = @idpadre
       and id not in (select id from @MenusAccedidos)
       
     FETCH menus INTO @idpadre
    end
    
    CLOSE menus
    DEALLOCATE menus    
  end
                   
 RETURN 
END
go
");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
