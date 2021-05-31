using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;

namespace ServicioDeDatos.Negocio
{
    [Table("NEGOCIO", Schema = "NEGOCIO")]
    public class NegocioDtm : RegistroConNombre
    {
        public string ElementoDtm { get; set; }
        public string ElementoDto { get; set; }

        [Column("ICONO", TypeName = "VARCHAR(250)")]
        public string Icono { get; set; }

        [Required]
        [Column("ACTIVO", TypeName = "BIT")]
        public bool Activo { get; set; }

        [Column("IDPERMISO_GESTOR", TypeName = "INT")]
        public int IdPermisoDeGestor { get; set; }

        [Column("IDPERMISO_CONSULTOR", TypeName = "INT")]
        public int IdPermisoDeConsultor { get; set; }

        [Column("IDPERMISO_ADMINISTRADOR", TypeName = "INT")]
        public int IdPermisoDeAdministrador { get; set; }

        public PermisoDtm PermisoDeGestor { get; set; }
        public PermisoDtm PermisoDeConsultor { get; set; }
        public PermisoDtm PermisoDeAdministrador { get; set; }

        public bool UsaSeguridad { get; set; }
        public bool EsDeParametrizacion { get; set; }
        public string Enumerado { get; set; }


    }

    public static class NegocioSqls
    {
        private static readonly string LeerNegocio = @"
SELECT [ID]
      ,[ELEMENTO_DTM] as ElementoDtm
      ,[ICONO]
      ,[ACTIVO]
      ,[IDPERMISO_GESTOR] as IdPermisoDeGestor
      ,[IDPERMISO_CONSULTOR] as IdPermisoDeConsultor
      ,[IDPERMISO_ADMINISTRADOR] as IdPermisoDeAdministrador
      ,[NOMBRE]
      ,[ELEMENTO_DTO] as ElementoDto
      ,[USA_SEGURIDAD] as UsaSeguridad
      ,[ES_DE_PARAMETRIZACION] as EsDeParametrizacion
      ,[ENUMERADO] as Enumerado
FROM [NEGOCIO].[NEGOCIO]";

        public static readonly string LeerNegocioPorNombre = $@"{LeerNegocio}{Environment.NewLine} WHERE [NOMBRE] like @Nombre";
        public static readonly string LeerNegocioPorEnumerado = $@"{LeerNegocio}{Environment.NewLine} WHERE [ENUMERADO] like @Enumerado";

        private static readonly string Actualizar = $@"
UPDATE [NEGOCIO].[NEGOCIO]
   SET [ELEMENTO_DTM] = @ElementoDtm
      ,[ICONO] =  @Icono
      ,[ACTIVO] = @Activo
      ,[IDPERMISO_GESTOR] = @IdPermisoGestor
      ,[IDPERMISO_CONSULTOR] = @IdPermisoConsultor
      ,[IDPERMISO_ADMINISTRADOR] = @IdPermisoAdministrador
      ,[NOMBRE] = @Nombre
      ,[ELEMENTO_DTO] = @ElementoDto
      ,[ENUMERADO] = @Enumerado
      ,[ES_DE_PARAMETRIZACION] = @EsDeParametrizacion
      ,[USA_SEGURIDAD] = @UsaSeguridad
 WHERE ID = @ID
";
    }

    public static class TablaNegocio
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NegocioDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<NegocioDtm>().Property(p => p.Activo).IsRequired(true).HasDefaultValue(true);
            modelBuilder.Entity<NegocioDtm>().Property(p => p.ElementoDtm).HasColumnName("ELEMENTO_DTM").HasColumnType("VARCHAR(250)").IsRequired(true);
            modelBuilder.Entity<NegocioDtm>().Property(p => p.ElementoDto).HasColumnName("ELEMENTO_DTO").HasColumnType("VARCHAR(250)").IsRequired(false);

            modelBuilder.Entity<NegocioDtm>().Property(p => p.UsaSeguridad).HasColumnName("USA_SEGURIDAD").HasColumnType("BIT").IsRequired(true).HasDefaultValue(true);
            modelBuilder.Entity<NegocioDtm>().Property(p => p.EsDeParametrizacion).HasColumnName("ES_DE_PARAMETRIZACION").HasColumnType("BIT").IsRequired(true).HasDefaultValue(false);

            modelBuilder.Entity<NegocioDtm>().Property(p => p.Enumerado).HasColumnName("ENUMERADO").HasColumnType("VARCHAR(250)").IsRequired(true);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeGestor)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeGestor)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_GESTOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeConsultor)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeConsultor)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_CONSULTOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeAdministrador)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeAdministrador)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_ADMINISTRADOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeAdministrador })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_ADMINISTRADOR");

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeConsultor })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_CONSULTOR");

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeGestor })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_GESTOR");

            modelBuilder.Entity<NegocioDtm>().HasIndex(x => x.Nombre)
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_NOMBRE");

        }
    }



}
