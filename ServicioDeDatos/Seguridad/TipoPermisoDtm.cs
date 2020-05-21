using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("TIPO_PERMISO", Schema = "SEGURIDAD")]
    public class TipoPermisoDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(30)")]
        public string Nombre { get; set; }

        public virtual ICollection<PermisoDtm> Permisos { get; set; }
    }

    public static class TablaPermisoTipo
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoPermisoDtm>()
                        .HasIndex(tp => tp.Nombre)
                        .HasName("I_TIPO_PERMISO_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<TipoPermisoDtm>()
                .HasMany(tp => tp.Permisos)
                .WithOne(p => p.Tipo);

        }
    }
}