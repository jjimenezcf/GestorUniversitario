﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace GestorDeEntorno.Migrations
{
    public partial class anadir_no_lock_al_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
USE [SistemaDeElementos]
GO
/****** Object:  UserDefinedFunction [ENTORNO].[ARBOL_MENU_POR_USUARIO]    Script Date: 06/04/2021 13:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [ENTORNO].[ARBOL_MENU_POR_USUARIO] (@IDUSUARIO INT)
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
    from ENTORNO.MENU t1 with(nolock)
    inner join ENTORNO.VISTA_MVC t2  with(nolock) on t2.id = t1.IDVISTA_MVC
    where t1.IDVISTA_MVC is not null
      and t1.ACTIVO = 1
      and exists (select 1 from ENTORNO.USU_PERMISO with(nolock) where IDUSUA = @IDUSUARIO and IDPERMISO = t2.IDPERMISO)                        
  
    declare menus  CURSOR for select IDPADRE from @MenusAccedidos 
       
    open menus
    FETCH menus INTO @idpadre
    WHILE (@@fetch_status = 0) 
    BEGIN
     insert into @MenusAccedidos  
     select T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN
     from ENTORNO.MENU t1 with(nolock)
     where id = @idpadre
       and id not in (select id from @MenusAccedidos)
       
     FETCH menus INTO @idpadre
    end
    
    CLOSE menus
    DEALLOCATE menus    
  end
                   
 RETURN 
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
