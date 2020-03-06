﻿// <auto-generated />
using System;
using Gestor.Elementos.Permiso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migraciones.Migrations
{
    [DbContext(typeof(CtoPermisos))]
    [Migration("20200306081148_RenombrarCursoGrupo")]
    partial class RenombrarCursoGrupo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Permiso.GrupoReg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Clase")
                        .HasColumnName("CLASE")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.ToTable("GRUPO","USUARIO");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeInscripcion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CursoId")
                        .HasColumnType("INT");

                    b.Property<int>("EstudianteId")
                        .HasColumnType("int");

                    b.Property<int?>("Grado")
                        .HasColumnType("int");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("EST_CURSO","UNIVERSIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.UsuarioReg", b =>
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

                    b.ToTable("USUARIO","USUARIO");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RegistroDeInscripcion", b =>
                {
                    b.HasOne("Gestor.Elementos.Permiso.GrupoReg", "Curso")
                        .WithMany("Inscripciones")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gestor.Elementos.Permiso.UsuarioReg", "Usuario")
                        .WithMany("Inscripciones")
                        .HasForeignKey("UsuarioId");
                });
#pragma warning restore 612, 618
        }
    }
}