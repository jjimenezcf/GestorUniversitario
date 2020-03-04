﻿// <auto-generated />
using System;
using Gestor.Elementos.Entorno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestorDeEntorno.Migrations
{
    [DbContext(typeof(ContextoEntorno))]
    [Migration("20200304202246_inicialEntorno")]
    partial class inicialEntorno
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Entorno.Fun_Accion", b =>
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

                    b.Property<string>("Parametros")
                        .IsRequired()
                        .HasColumnName("PARAMETROS")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("FUN_ACCION","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.Fun_Elemento", b =>
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

                    b.Property<int?>("IDACCION")
                        .HasColumnType("INT");

                    b.Property<int>("IdPadre")
                        .HasColumnName("IDPADRE")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IDACCION");

                    b.ToTable("FUN_ELEMENTO","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.Var_Elemento", b =>
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

                    b.ToTable("VAR_ELEMENTO","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Entorno.Fun_Elemento", b =>
                {
                    b.HasOne("Gestor.Elementos.Entorno.Fun_Accion", "Accion")
                        .WithMany()
                        .HasForeignKey("IDACCION");
                });
#pragma warning restore 612, 618
        }
    }
}
