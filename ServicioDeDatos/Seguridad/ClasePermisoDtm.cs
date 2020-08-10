using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("CLASE_PERMISO", Schema = "SEGURIDAD")]
    public class ClasePermisoDtm : Registro
    {
        public virtual ICollection<PermisoDtm> Permisos { get; set; }
    }

    public static class TablaClasePermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClasePermisoDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(30)").IsRequired();
            modelBuilder.Entity<ClasePermisoDtm>()
                        .HasIndex(cp => cp.Nombre)
                        .HasName("I_CLASE_PERMISO_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<ClasePermisoDtm>()
                .HasMany(cp => cp.Permisos)
                .WithOne(p => p.Clase)
                .HasForeignKey(p=>p.IdClase);

        }
    }
}
