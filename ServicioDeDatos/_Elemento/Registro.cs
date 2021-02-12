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


    public class Registro
    {
        [Key]
        [Column("ID", Order = 1, TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

    public static class RegistroExtensiones
    {
        public static object ValorPropiedad(this Registro registro, string propiedad)
        {
            return registro.GetType().GetProperty(propiedad).GetValue(registro);
        }

        public static bool ImplementaNombre(this Registro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(INombre));
        }
        public static bool ImplementaNombre(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(INombre));
        }
        public static bool ImplementaUnaRelacion(this Registro registro)
        {
            return registro.GetType().GetInterfaces().Contains(typeof(IRelacion));
        }
        public static bool ImplementaUnaRelacion(this Type tipoRegistro)
        {
            return tipoRegistro.GetInterfaces().Contains(typeof(IRelacion));
        }
        public static bool ImplementaUnElemento(this Registro registro)
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