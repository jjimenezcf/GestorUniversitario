﻿using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("PUESTO", Schema = "SEGURIDAD")]
    public class PuestoDtm : Registro
    {
        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public virtual ICollection<RolesDeUnPuestoDtm> Roles { get; set; }
        public virtual ICollection<PuestoDeUnUsuarioDtm> Usuarios { get; set; }
    }

    public static class TablaPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PuestoDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<PuestoDtm>()
            .HasIndex(p => p.Nombre)
            .HasName("I_PUESTO_NOMBRE")
            .IsUnique();


            modelBuilder.Entity<PuestoDtm>()
                    .HasMany(p => p.Usuarios)
                    .WithOne(p => p.Puesto)
                    .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<PuestoDtm>()
                    .HasMany(p => p.Roles)
                    .WithOne(p => p.Puesto)
                    .HasForeignKey(p => p.IdRol);

        }
    }
}
