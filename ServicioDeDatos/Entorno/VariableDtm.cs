using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Entorno
{
    [Table("VARIABLE", Schema = "ENTORNO")]
    public class VariableDtm : RegistroConNombre
    {
        [Column("DESCRIPCION", Order = 4, TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("VALOR", Order = 3, TypeName = "VARCHAR(250)")]
        public string Valor { get; set; }
    }

    public static class TablaVariable
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VariableDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(50)").IsRequired();
            modelBuilder.Entity<VariableDtm>().Property(v => v.Descripcion).IsRequired(false);
            modelBuilder.Entity<VariableDtm>().HasIndex(v => new { v.Nombre }).IsUnique(true).HasDatabaseName("IND_VARIABLE_NOMBRE");

        }
    }


    public static class VariableSqls
    {
        public static string CrearVariable ="INSERT INTO SistemaDeElementos.ENTORNO.VARIABLE (NOMBRE, DESCRIPCION, VALOR) VALUES(@variable, @descripcion, @valor)"; 
        
        public static string ModificarVariable = "UPDATE SistemaDeElementos.ENTORNO.VARIABLE SET VALOR = @valor WHERE NOMBRE like @variable";

        public static string LeerValorDeVariable ="SELECT ID, NOMBRE, DESCRIPCION, VALOR FROM SistemaDeElementos.ENTORNO.VARIABLE WHERE NOMBRE like @variable";

    }
}
