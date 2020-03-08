/*==============================================================*/
/* Database name:  SISTEMADEELEMENTOS                           */
/* DBMS name:      Microsoft SQL Server 2014                    */
/* Created on:     08/03/2020 13:07:17                          */
/*==============================================================*/


drop database SISTEMADEELEMENTOS
go

/*==============================================================*/
/* Database: SISTEMADEELEMENTOS                                 */
/*==============================================================*/
create database SISTEMADEELEMENTOS
go

use SISTEMADEELEMENTOS
go

/*==============================================================*/
/* User: ADMIN                                                  */
/*==============================================================*/
create schema ADMIN
go

/*==============================================================*/
/* User: ADMIN                                                  */
/*==============================================================*/
execute sp_grantdbaccess ADMIN
go

/*==============================================================*/
/* User: ENTORNO                                                */
/*==============================================================*/
create schema ENTORNO authorization ADMIN
go

/*==============================================================*/
/* User: SEGURIDAD                                              */
/*==============================================================*/
create schema SEGURIDAD authorization ADMIN
go

/*==============================================================*/
/* Table: CENTROGESTOR                                          */
/*==============================================================*/
create table CENTROGESTOR (
   ID                   int                  identity
)
go

/*==============================================================*/
/* Table: CG_PERMISO                                            */
/*==============================================================*/
create table CG_PERMISO (
   ID                   int                  identity,
   IDPERMISO            int                  identity,
   IDCG                 int                  not null
)
go

/*==============================================================*/
/* Table: EXP_ESTADO                                            */
/*==============================================================*/
create table EXP_ESTADO (
   ID                   int                  identity
)
go

/*==============================================================*/
/* Table: EXP_EST_PERMISO                                       */
/*==============================================================*/
create table EXP_EST_PERMISO (
   ID                   int                  identity,
   IDPERMISO            int                  not null,
   IDESTADO             int                  not null
)
go

/*==============================================================*/
/* Table: EXP_TIPO                                              */
/*==============================================================*/
create table EXP_TIPO (
   ID                   int                  identity
)
go

/*==============================================================*/
/* Table: EXP_TIP_PERMISO                                       */
/*==============================================================*/
create table EXP_TIP_PERMISO (
   ID                   int                  identity,
   IDPERMISO            int                  not null,
   IDTIPO               int                  not null
)
go

/*==============================================================*/
/* Table: PERMISO                                               */
/*==============================================================*/
create table SEGURIDAD.PERMISO (
   ID                   int                  identity,
   NOMBRE               varchar(250)         not null,
   CLASE                decimal(2,0)         not null,
   PERMISO              decimal(2,0)         not null
)
go

alter table SEGURIDAD.PERMISO
   add constraint PK_PERMISO primary key (ID)
go

alter table SEGURIDAD.PERMISO
   add constraint AK_PERMISO_NOMBRE unique (NOMBRE)
go

/*==============================================================*/
/* Table: PUESTO                                                */
/*==============================================================*/
create table SEGURIDAD.PUESTO (
   ID                   int                  identity
)
go

/*==============================================================*/
/* Table: ROL                                                   */
/*==============================================================*/
create table SEGURIDAD.ROL (
   ID                   int                  identity,
   NOMBRE               varchar(250)         not null,
   DESCRIPCION          varchar(MAX)         null
)
go

/*==============================================================*/
/* Table: ROL_PERMISO                                           */
/*==============================================================*/
create table SEGURIDAD.ROL_PERMISO (
   ID                   int                  identity,
   IDROL                int                  not null,
   IDPERMISO            int                  not null
)
go

/*==============================================================*/
/* Table: ROL_PUESTO                                            */
/*==============================================================*/
create table SEGURIDAD.ROL_PUESTO (
   ID                   int                  identity,
   IDROL                int                  not null,
   IDPUESTO             int                  identity
)
go

/*==============================================================*/
/* Table: USUARIO                                               */
/*==============================================================*/
create table ENTORNO.USUARIO (
   ID                   int                  identity
)
go

alter table ENTORNO.USUARIO
   add constraint PK_USUARIO primary key (ID)
go

/*==============================================================*/
/* Table: USU_PUESTO                                            */
/*==============================================================*/
create table SEGURIDAD.USU_PUESTO (
   ID                   int                  not null,
   IDUSUA               int                  not null,
   IDPUESTO             int                  not null
)
go

alter table CG_PERMISO
   add constraint FK_CG_PERMI_F_CG_PERM_CENTROGE foreign key (IDCG)
      references CENTROGESTOR (ID)
go

alter table CG_PERMISO
   add constraint FK_CG_PERMI_F_CG_PERM_PERMISO foreign key (IDCG)
      references SEGURIDAD.PERMISO (ID)
go

alter table EXP_EST_PERMISO
   add constraint FK_EXP_EST__F_EXP_EST_EXP_ESTA foreign key (IDESTADO)
      references EXP_ESTADO (ID)
go

alter table EXP_EST_PERMISO
   add constraint FK_EXP_EST__F_EXP_EST_PERMISO foreign key (IDPERMISO)
      references SEGURIDAD.PERMISO (ID)
go

alter table EXP_TIP_PERMISO
   add constraint FK_EXP_TIP__F_EXP_TIP_PERMISO foreign key (IDPERMISO)
      references SEGURIDAD.PERMISO (ID)
go

alter table EXP_TIP_PERMISO
   add constraint FK_EXP_TIP__F_EXP__TI_EXP_TIPO foreign key (IDTIPO)
      references EXP_TIPO (ID)
go

alter table SEGURIDAD.ROL_PERMISO
   add constraint FK_ROL_PERM_F_ROL_PER_PERMISO foreign key (IDPERMISO)
      references SEGURIDAD.PERMISO (ID)
go

alter table SEGURIDAD.ROL_PERMISO
   add constraint FK_ROL_PERM_F_ROL_PER_ROL foreign key (IDROL)
      references SEGURIDAD.ROL (ID)
go

alter table SEGURIDAD.ROL_PUESTO
   add constraint f_rol_puesto_idRol foreign key (IDROL)
      references SEGURIDAD.ROL (ID)
go

alter table SEGURIDAD.ROL_PUESTO
   add constraint FK_ROL_PUES_F_ROL_PUE_PUESTO foreign key ()
      references SEGURIDAD.PUESTO
go

alter table SEGURIDAD.USU_PUESTO
   add constraint idpuesto foreign key (IDPUESTO)
      references SEGURIDAD.PUESTO (ID)
go

alter table SEGURIDAD.USU_PUESTO
   add constraint FK_USU_PUES_F_USU_PUE_USUARIO foreign key (IDUSUA)
      references ENTORNO.USUARIO (ID)
go

