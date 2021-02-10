using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Elemento
{
    public class GeneradorMd
    {
        private static string NombreDeTabla(Type t)
        {

            Attribute[] attrs = Attribute.GetCustomAttributes(t);

            foreach (Attribute attr in attrs)
            {
                if (attr is TableAttribute)
                {
                    var tabla = (TableAttribute)attr;
                    return tabla.Name;
                }
            }

            throw new Exception($"No se ha definido el nombre de la tabla de la clase {t.Name}");
        }

        internal static void DefinirCampoArchivo<TEntity>(ModelBuilder modelBuilder) where TEntity : Registro
        {
            var nombreDeTabla = NombreDeTabla(typeof(TEntity));


            modelBuilder.Entity<TEntity>().Property("IdArchivo").HasColumnName("IDARCHIVO");
            modelBuilder.Entity<TEntity>().Property("IdArchivo").HasColumnType("INT");
            modelBuilder.Entity<TEntity>().Property("IdArchivo").IsRequired(false);



            modelBuilder.Entity<TEntity>()
                        .HasIndex("IdArchivo")
                        .HasDatabaseName($"I_{nombreDeTabla}_IDARCHIVO");

            modelBuilder.Entity<TEntity>()
                        .HasOne("Archivo")
                        .WithMany()
                        .HasForeignKey("IdArchivo")
                        .HasConstraintName($"FK_{nombreDeTabla}_IDARCHIVO")
                        .OnDelete(DeleteBehavior.Restrict);
        }

        internal static void DefinirCamposDelElementoDtm<TEntity>(ModelBuilder modelBuilder) where TEntity : ElementoDtm
        {
            var nombreDeTabla = NombreDeTabla(typeof(TEntity));

            modelBuilder.Entity<TEntity>()
                        .HasIndex(p => p.Nombre)
                        .HasDatabaseName($"I_{nombreDeTabla}_NOMBRE");

            modelBuilder.Entity<TEntity>()
           .HasOne(p => p.UsuarioCreador)
           .WithMany()
           .HasForeignKey(p => p.IdUsuaCrea)
           .HasConstraintName($"FK_{nombreDeTabla}_IDUSUCREA")
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TEntity>()
            .HasOne(p => p.UsuarioModificador)
            .WithMany()
            .HasForeignKey(p => p.IdUsuaModi)
            .HasConstraintName($"FK_{nombreDeTabla}_IDUSUMODI")
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TEntity>()
                        .HasIndex(p => p.IdUsuaCrea)
                        .HasDatabaseName($"I_{nombreDeTabla}_IDUSUCREA");

            modelBuilder.Entity<TEntity>()
                        .HasIndex(p => p.IdUsuaModi)
                        .HasDatabaseName($"I_{nombreDeTabla}_IDUSUMODI");

        }
    }

    public interface INombre
    {
        public string Nombre { get; set; }
    }

    public interface IElementoDtm: INombre
    {
        public DateTime FechaCreacion { get; set; }
        public int IdUsuaCrea { get; set; }
        public UsuarioDtm UsuarioCreador { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsuaModi { get; set; }
        public UsuarioDtm UsuarioModificador { get; set; }
    }


    public class Registro
    {
        [Key]
        [Column("ID", Order = 1, TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public string Nombre { get; set; }


        [IgnoreDataMember]
        [NotMapped]
        public bool RegistroDeRelacion { get; set; } = false;


        [IgnoreDataMember]
        [NotMapped]
        public bool NombreObligatorio { get; set; } = true;


        [IgnoreDataMember]
        [NotMapped]
        public bool RegistroConAuditoria { get; set; } = false;

        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento1 { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento2 { get; set; }

        public static TRegistro RegistroVacio<TRegistro>()
        {
            var className = typeof(TRegistro).FullName;
            var assembly = Assembly.GetAssembly(Type.GetType(className));
            var type = assembly.GetType(className);

            //Constructor genérico           
            var constructorSinParametros = type.GetConstructor(Type.EmptyTypes);

            //Creamos el objeto de manera dinámica
            return (TRegistro)constructorSinParametros.Invoke(new object[] { });
        }

    }


    public class RegistroDeRelacion : Registro
    {
        public RegistroDeRelacion()
        {
            RegistroDeRelacion = true;
            NombreObligatorio = false;
        }
    }

    public class ElementoDtm : Registro, IElementoDtm, INombre
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public new string Nombre { get; set; }

        [Required]
        [Column("FECCRE", Order = 1, TypeName = "DATETIME")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Column("IDUSUCREA", Order = 1, TypeName = "INT")]
        public int IdUsuaCrea { get; set; }

        public virtual UsuarioDtm UsuarioCreador { get; set; }

        [Column("FECMOD", Order = 1, TypeName = "DATETIME")]
        public DateTime? FechaModificacion { get; set; }

        [Column("IDUSUMODI", Order = 1, TypeName = "INT")]
        public int? IdUsuaModi { get; set; }
        public virtual UsuarioDtm UsuarioModificador { get; set; }

        public ElementoDtm()
        {
            RegistroConAuditoria = true;            
        }
    }

}