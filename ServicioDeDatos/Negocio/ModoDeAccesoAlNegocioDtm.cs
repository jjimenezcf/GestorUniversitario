﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Negocio
{
    [Table("MODO_ACCESO_NEGOCIO", Schema = "NEGOCIO")]
    public class ModoDeAccesoAlNegocioDtm : Registro
    {
        public bool Administrador { get; set; }
        public bool Gestor { get; set; }
        public bool Consultor { get; set; }

        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsuario { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public string Origen { get; set; }
    }

    public static class VistaModoDeAccesoAlNegocio
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModoDeAccesoAlNegocioDtm>()
                .ToTable("MODO_ACCESO_NEGOCIO", "NEGOCIO")
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<ModoDeAccesoAlNegocioDtm>().Property(p => p.Administrador).HasColumnName("ADMINISTRADOR").HasColumnType("BIT");
            modelBuilder.Entity<ModoDeAccesoAlNegocioDtm>().Property(p => p.Gestor).HasColumnName("GESTOR").HasColumnType("BIT");
            modelBuilder.Entity<ModoDeAccesoAlNegocioDtm>().Property(p => p.Consultor).HasColumnName("CONSULTOR").HasColumnType("BIT");

            modelBuilder.Entity<PermisosDeUnUsuarioDtm>().Property(p => p.Origen).HasColumnName("ORIGEN").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("SEGURIDAD.OBTENER_ORIGEN(idusua,idpermiso)");

        }
    }
}
