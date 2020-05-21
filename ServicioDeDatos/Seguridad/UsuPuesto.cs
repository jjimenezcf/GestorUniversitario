using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Seguridad
{

    public class vUsuario : Registro
    {
        [Column("LOGIN", Order = 1, TypeName = "VARCHAR(50)")]
        public string Login { get; set; }

        [Column("APELLIDO", Order = 2, TypeName = "VARCHAR(250)")]
        public string Apellido { get; set; }

        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }

        public ICollection<RegUsuPuesto> Puestos { get; set; }

    }

    public static class VistaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vUsuario>()
                .ToView("V_USUARIO", "SEGURIDAD")
                .HasNoKey();
        }
    }



    [Table("USU_PUESTO", Schema = "SEGURIDAD")]
    public class RegUsuPuesto : Registro
    {
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsua { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public rPuesto Puesto { get; set; }
        public vUsuario Usuario { get; set; }
    }

    public static class TablaUsuPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<RegUsuPuesto>()
                .HasAlternateKey(p => new { p.IdUsua, p.idPuesto })
                .HasName("AK_USU_PUESTO");

            modelBuilder.Entity<RegUsuPuesto>()
                .HasIndex(p => p.IdUsua)
                .IsUnique(false)
                .HasName("IX_USU_PUESTO_IDUSUA");

            modelBuilder.Entity<RegUsuPuesto>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_USU_PUESTO_IDPUESTO");

            modelBuilder.Entity<RegUsuPuesto>()
                .Ignore(x => x.Usuario);

        }
    }

}
