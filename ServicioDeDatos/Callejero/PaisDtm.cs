﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;

namespace ServicioDeDatos.Callejero
{
    [Table("PAIS", Schema = "CALLEJERO")]
    public class PaisDtm : ElementoDtm
    {
        public string Name { get; set; }
        public string Codigo { get; set; }
        public string ISO2 { get; set; }
        public string Prefijo {get; set;}
    }

    public static class TablaPais
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirCamposDelElementoDtm<PaisDtm>(modelBuilder);
            modelBuilder.Entity<PaisDtm>().Property(v => v.Codigo)
                .HasColumnName("CODIGO")
                .HasColumnType("VARCHAR(3)")
                .IsRequired(true);

            modelBuilder.Entity<PaisDtm>().Property(v => v.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(250)")
                .IsRequired(true);

            modelBuilder.Entity<PaisDtm>().Property(v => v.ISO2)
                .HasColumnName("ISO2")
                .HasColumnType("VARCHAR(2)")
                .IsRequired(true);

            modelBuilder.Entity<PaisDtm>().Property(v => v.Prefijo)
                .HasColumnName("PREFIJO")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);
        }
    }

}
