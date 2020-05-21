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
    [Migration("20200321224020_AjustarIndicesEnEntorno")]
    partial class AjustarIndicesEnEntorno
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Entorno.RegAccion", b =>
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
                        .HasName("IX_ACCION")
                        .HasFilter("[PARAMETROS] IS NOT NULL");

                    b.ToTable("ACCION","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.RegFuncion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Activo")
                        .IsRequired()
                        .HasColumnName("ACTIVO")
                        .HasColumnType("CHAR(1)");

                    b.Property<string>("Descripcion")
                        .HasColumnName("DESCRIPCION")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<string>("ICONO")
                        .IsRequired()
                        .HasColumnName("ICONO")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int?>("IdAccion")
                        .HasColumnName("IDACCION")
                        .HasColumnType("INT");

                    b.Property<int?>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdAccion");

                    b.HasIndex("IdPadre");

                    b.ToTable("FUNCION","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.RegUsuario", b =>
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

            modelBuilder.Entity("Gestor.Elementos.Entorno.RegVariable", b =>
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

            modelBuilder.Entity("Gestor.Elementos.Entorno.RegFuncion", b =>
                {
                    b.HasOne("Gestor.Elementos.Entorno.RegAccion", "Accion")
                        .WithMany("Funciones")
                        .HasForeignKey("IdAccion")
                        .HasConstraintName("FK_FUNCION_IDACCION");

                    b.HasOne("Gestor.Elementos.Entorno.RegFuncion", "Padre")
                        .WithMany()
                        .HasForeignKey("IdPadre")
                        .HasConstraintName("FK_FUNCION_IDPADRE");
                });
#pragma warning restore 612, 618
        }
    }
}
