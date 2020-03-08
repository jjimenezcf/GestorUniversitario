using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

/*
create table SEGURIDAD.USU_PUESTO (
   ID                   int                  not null,
   IDUSUA               int                  not null,
   IDPUESTO             int                  not null
)
go

alter table SEGURIDAD.USU_PUESTO
   add constraint PK_USU_PUESTO primary key (ID)
go

alter table SEGURIDAD.USU_PUESTO
   add constraint AK_USU_PUESTO unique (IDUSUA, IDPUESTO)
go

alter table SEGURIDAD.USU_PUESTO
   add constraint FK_USU_PUESTO_IDPUESTO foreign key (IDPUESTO)
      references SEGURIDAD.PUESTO (ID)
go

alter table SEGURIDAD.USU_PUESTO
   add constraint FK_USU_PUESTO_IDUSUARIO foreign key (IDUSUA)
      references ENTORNO.USUARIO (ID)
go
     
 */


namespace Gestor.Elementos.Seguridad
{

    public class UsuarioView : Registro
    {
        [Column("LOGIN", Order = 1, TypeName = "VARCHAR(50)")]
        public string Login { get; set; }

        [Column("APELLIDO", Order = 2, TypeName = "VARCHAR(250)")]
        public string Apellido { get; set; }

        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }

        public ICollection<UsuPuestoReg> Puestos { get; set; }

    }

    public static class VistaUsuario
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioView>()
                .ToView("V_USUARIO", "SEGURIDAD")
                .HasNoKey();
        }
    }



    [Table("USU_PUESTO", Schema = "SEGURIDAD")]
    public class UsuPuestoReg : Registro
    {
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsua { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public PuestoReg Puesto { get; set; }
        public UsuarioView Usuario { get; set; }
    }

    public static class TablaUsuPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UsuPuestoReg>()
                .HasAlternateKey(p => new { p.IdUsua, p.idPuesto })
                .HasName("AK_USU_PUESTO");

            modelBuilder.Entity<UsuPuestoReg>()
                .HasIndex(p => p.IdUsua)
                .IsUnique(false)
                .HasName("IX_USU_PUESTO_IDUSUA");

            modelBuilder.Entity<UsuPuestoReg>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_USU_PUESTO_IDPUESTO");

            modelBuilder.Entity<UsuPuestoReg>()
                .Ignore(x => x.Usuario)
                //.HasOne(x => x.Usuario)
                //.WithMany(u => u.Puestos)
                //.HasForeignKey(x => x.Usuario)
                //.HasConstraintName("FK_USU_PUESTO_IDUSUA")
                ;

        }
    }

}
