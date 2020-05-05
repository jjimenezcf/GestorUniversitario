﻿// <auto-generated />
using Gestor.Elementos.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Migraciones.Migrations
{
    [DbContext(typeof(CtoSeguridad))]
    [Migration("20200505091959_modificat_permiso")]
    partial class modificat_permiso
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gestor.Elementos.Seguridad.ClasePermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_CLASE_PERMISO_NOMBRE");

                    b.ToTable("CLASE_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.PerUsuarioDtm", b =>
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

                    b.HasIndex("IdPermiso");

                    b.ToTable("USU_PERMISO","ENTORNO");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.PermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdClase")
                        .HasColumnName("IDCLASE")
                        .HasColumnType("INT");

                    b.Property<int>("IdTipo")
                        .HasColumnName("IDTIPO")
                        .HasColumnType("INT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdTipo");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_PERMISO_NOMBRE");

                    b.HasIndex("IdClase", "IdTipo")
                        .HasName("I_PERMISO_IDCLASE_IDTIPO");

                    b.ToTable("PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.RegUsuPuesto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdUsua")
                        .HasColumnName("IDUSUA")
                        .HasColumnType("INT");

                    b.Property<int>("idPuesto")
                        .HasColumnName("IDPUESTO")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasAlternateKey("IdUsua", "idPuesto")
                        .HasName("AK_USU_PUESTO");

                    b.HasIndex("IdUsua")
                        .HasName("IX_USU_PUESTO_IDUSUA");

                    b.HasIndex("idPuesto");

                    b.ToTable("USU_PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.TipoPermisoDtm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnName("NOMBRE")
                        .HasColumnType("VARCHAR(30)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasName("I_TIPO_PERMISO_NOMBRE");

                    b.ToTable("TIPO_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rPuesto", b =>
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
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Nombre")
                        .HasName("AK_PUESTO_NOMBRE");

                    b.ToTable("PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rRol", b =>
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
                        .HasColumnType("VARCHAR(250)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Nombre")
                        .HasName("AK_ROL_NOMBRE");

                    b.ToTable("ROL","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rRolPermiso", b =>
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

                    b.HasKey("Id");

                    b.HasAlternateKey("IdRol", "IdPermiso")
                        .HasName("AK_ROL_PERMISO");

                    b.HasIndex("IdPermiso");

                    b.ToTable("ROL_PERMISO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rRolPuesto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("INT")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdRol")
                        .HasColumnName("IDROL")
                        .HasColumnType("INT");

                    b.Property<int>("idPuesto")
                        .HasColumnName("IDPUESTO")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasAlternateKey("IdRol", "idPuesto")
                        .HasName("AK_ROL_PUESTO");

                    b.HasIndex("idPuesto");

                    b.ToTable("ROL_PUESTO","SEGURIDAD");
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.PerUsuarioDtm", b =>
                {
                    b.HasOne("Gestor.Elementos.Seguridad.PermisoDtm", "Permiso")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.PermisoDtm", b =>
                {
                    b.HasOne("Gestor.Elementos.Seguridad.ClasePermisoDtm", "Clase")
                        .WithMany("Permisos")
                        .HasForeignKey("IdClase")
                        .HasConstraintName("FK_PERMISO_IDCLASE")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gestor.Elementos.Seguridad.TipoPermisoDtm", "Tipo")
                        .WithMany("Permisos")
                        .HasForeignKey("IdTipo")
                        .HasConstraintName("FK_PERMISO_IDTIPO")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.RegUsuPuesto", b =>
                {
                    b.HasOne("Gestor.Elementos.Seguridad.rPuesto", "Puesto")
                        .WithMany("Usuarios")
                        .HasForeignKey("idPuesto")
                        .HasConstraintName("FK_USU_PUESTO_IDPUESTO")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rRolPermiso", b =>
                {
                    b.HasOne("Gestor.Elementos.Seguridad.PermisoDtm", "Permiso")
                        .WithMany("Roles")
                        .HasForeignKey("IdPermiso")
                        .HasConstraintName("FK_ROL_PERMISO_IDPERMISO")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gestor.Elementos.Seguridad.rRol", "Rol")
                        .WithMany("Permisos")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK_ROL_PERMISO_IDROL")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gestor.Elementos.Seguridad.rRolPuesto", b =>
                {
                    b.HasOne("Gestor.Elementos.Seguridad.rRol", "Rol")
                        .WithMany("Puestos")
                        .HasForeignKey("IdRol")
                        .HasConstraintName("FK_ROL_PUESTO_IDROL")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gestor.Elementos.Seguridad.rPuesto", "Puesto")
                        .WithMany("Roles")
                        .HasForeignKey("idPuesto")
                        .HasConstraintName("FK_ROL_PUESTO_IDPUESTO")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
