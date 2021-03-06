﻿// <auto-generated />
using System;
using ServicioDeDatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migraciones.Migrations
{
    [DbContext(typeof(ContextoSe))]
    [Migration("20200308124534_CREAR_TABLA_ROL_pERMISO")]
    partial class CREAR_TABLA_ROL_pERMISO
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Permiso.PermisoReg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Clase")
                        .HasColumnName("CLASE")
                        .HasColumnType("decimal(2,0)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<decimal>("Permiso")
                        .HasColumnName("PERMISO")
                        .HasColumnType("decimal(2,0)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Nombre")
                        .HasName("AK_PERMISO_NOMBRE");

                    b.ToTable("PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RolPermisoReg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdPermiso")
                        .HasColumnName("IDPERMISO")
                        .HasColumnType("INT");

                    b.Property<int>("IdRol")
                        .HasColumnName("IDROL")
                        .HasColumnType("INT");

                    b.Property<int?>("PermisoId")
                        .HasColumnType("INT");

                    b.Property<int?>("RolesId")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("PermisoId");

                    b.HasIndex("RolesId");

                    b.ToTable("ROL_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RolReg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Nombre")
                        .HasName("AK_ROL_NOMBRE");

                    b.ToTable("ROL","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Permiso.RolPermisoReg", b =>
                {
                    b.HasOne("Gestor.Elementos.Permiso.PermisoReg", "Permiso")
                        .WithMany("Roles")
                        .HasForeignKey("PermisoId")
                        .HasConstraintName("FK_ROL_PERMISO_IDPERMISO");

                    b.HasOne("Gestor.Elementos.Permiso.RolReg", "Roles")
                        .WithMany("Permisos")
                        .HasForeignKey("RolesId")
                        .HasConstraintName("FK_ROL_PERMISO_IDROL");
                });
#pragma warning restore 612, 618
        }
    }
}
