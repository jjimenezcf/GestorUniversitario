﻿// <auto-generated />
using System;
using ServicioDeDatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestorDeEntorno.Migrations
{
    [DbContext(typeof(ContextoDeElementos))]
    [Migration("20200513211849_arbolDeMenu")]
    partial class arbolDeMenu
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Entorno.MenuDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo")
                        .HasColumnName("ACTIVO")
                        .HasColumnType("BIT");

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Icono")
                        .IsRequired()
                        .HasColumnName("ICONO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int?>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<int?>("IdVistaMvc")
                        .HasColumnName("IDVISTA_MVC")
                        .HasColumnType("INT");

                    b.Property<int?>("MenuSeDtmId")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("Orden")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ORDEN")
                        .HasColumnType("INT")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("IdPadre");

                    b.HasIndex("IdVistaMvc");

                    b.HasIndex("MenuSeDtmId");

                    b.ToTable("MENU","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.MenuSeDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activo")
                        .HasColumnName("ACTIVO")
                        .HasColumnType("BIT");

                    b.Property<string>("Controlador")
                        .HasColumnName("CONTROLADOR")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Icono")
                        .IsRequired()
                        .HasColumnName("ICONO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int?>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<int?>("IdVistaMvc")
                        .HasColumnName("IDVISTA_MVC")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("Orden")
                        .HasColumnName("ORDEN")
                        .HasColumnType("INT");

                    b.Property<string>("Padre")
                        .HasColumnName("PADRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Vista")
                        .HasColumnName("VISTA")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("accion")
                        .HasColumnName("ACCION")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("parametros")
                        .HasColumnName("PARAMETROS")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("MENU_SE","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.UsuPermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdPermiso")
                        .HasColumnName("IDPERMISO")
                        .HasColumnType("INT");

                    b.Property<int>("IdUsua")
                        .HasColumnName("IDUSUA")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("IdUsua");

                    b.ToTable("USU_PERMISO","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.UsuarioDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Alta")
                        .HasColumnName("F_ALTA")
                        .HasColumnType("DATE");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnName("APELLIDO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("LOGIN")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasName("IX_USUARIO");

                    b.ToTable("USUARIO","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.VariableDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Valor")
                        .IsRequired()
                        .HasColumnName("VALOR")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.ToTable("VARIABLE","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.VistaMvcDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasColumnName("ACCION")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Controlador")
                        .IsRequired()
                        .HasColumnName("CONTROLADOR")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Parametros")
                        .HasColumnName("PARAMETROS")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("IX_VARIABLE");

                    b.HasIndex("Controlador", "Accion", "Parametros")
                        .IsUnique()
                        .HasName("IX_VISTA_MVC")
                        .HasFilter("[PARAMETROS] IS NOT NULL");

                    b.ToTable("VISTA_MVC","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.MenuDtm", b =>
                {
                    b.HasOne("Gestor.Elementos.Entorno.MenuDtm", "Padre")
                        .WithMany("Submenus")
                        .HasForeignKey("IdPadre")
                        .HasConstraintName("FK_MENU_IDPADRE");

                    b.HasOne("Gestor.Elementos.Entorno.VistaMvcDtm", "VistaMvc")
                        .WithMany("Menus")
                        .HasForeignKey("IdVistaMvc")
                        .HasConstraintName("FK_MENU_IDVISTA_MVC");

                    b.HasOne("Gestor.Elementos.Entorno.MenuSeDtm", null)
                        .WithMany("Submenus")
                        .HasForeignKey("MenuSeDtmId");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.UsuPermisoDtm", b =>
                {
                    b.HasOne("Gestor.Elementos.Entorno.UsuarioDtm", "Usuario")
                        .WithMany("Permisos")
                        .HasForeignKey("IdUsua")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
