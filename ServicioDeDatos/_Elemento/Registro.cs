using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Elemento
{
    public class GeneradorMd
    {
        public static string NombreDeTabla(Type t)
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

        public static string EsquemaDeTabla(Type t)
        {

            Attribute[] attrs = Attribute.GetCustomAttributes(t);

            foreach (Attribute attr in attrs)
            {
                if (attr is TableAttribute)
                {
                    var tabla = (TableAttribute)attr;
                    return tabla.Schema;
                }
            }

            throw new Exception($"No se ha definido el esquema de la tabla de la clase {t.Name}");
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

    public interface IElementoDtm : INombre
    {
        public DateTime FechaCreacion { get; set; }
        public int IdUsuaCrea { get; set; }
        public UsuarioDtm UsuarioCreador { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsuaModi { get; set; }
        public UsuarioDtm UsuarioModificador { get; set; }
    }

    public interface IRelacion
    {

        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento1 { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento2 { get; set; }
    }
    public interface IRegistro
    {
        public int Id { get; set; }
    }

    public class Registro: IRegistro
    {
        [Key]
        [Column("ID", Order = 1, TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public static class ApiDeRegistro
    {
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

        public static object ValorPropiedad(this IRegistro registro, string propiedad)
        {
            return registro.GetType().GetProperty(propiedad).GetValue(registro);
        }

        public static bool ImplementaNombre(this IRegistro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(INombre));
        }
        public static bool ImplementaNombre(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(INombre));
        }
        public static bool ImplementaUnaRelacion(this IRegistro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(IRelacion));
        }
        public static bool ImplementaUnaRelacion(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(IRelacion));
        }
        public static bool ImplementaUnElemento(this IRegistro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(IElementoDtm));
        }
        public static bool ImplementaUnElemento(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(IElementoDtm));
        }
    }

    public class RegistroConNombre : Registro, INombre
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }
    }

    public class RegistroDeRelacion : Registro, IRelacion
    {
        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento1 { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public string NombreDeLaPropiedadDelIdElemento2 { get; set; }
    }

    public class ElementoDtm : RegistroConNombre, IElementoDtm
    {
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
    }

}