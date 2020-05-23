using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Archivos
{
    [Table("ARCHIVO", Schema = "SISDOC")]
    public class ArchivoDtm : ElementoDtm
    {
        [Required]
        [Column("RUTA", TypeName = "VARCHAR(2000)")]
        public string AlmacenadoEn { get; set; }

    }

    public static class TablaArchivo
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirElementoDto<ArchivoDtm>(modelBuilder);
        }

    }
}
