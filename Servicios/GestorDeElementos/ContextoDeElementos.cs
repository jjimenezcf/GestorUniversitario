﻿using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using System.Linq;

namespace Gestor.Elementos
{
    class Literal
    {
        internal static string version = "Versión";
    }


    public class DatosDeConexion
    {
        public string ServidorWeb { get; set; }
        public string ServidorBd { get; set; }
        public string Bd { get; set; }
        public string Usuario { get; set; }
        public string Version { get; set; }
    }

    public class ContextoDeElementos : DbContext
    {
        public DatosDeConexion DatosDeConexion { get; set; }

        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {
            DatosDeConexion = new DatosDeConexion();
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Version = new ExisteTabla(this, "Var_Variable").Existe ?
             Variables.SingleOrDefault(v => v.Nombre == Literal.version).Valor :
             "0.0.0.";
            DatosDeConexion.Usuario = "jjimenezcf@gmail.com";
        }

       public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }
       public DbSet<RegistroDeVariable> Variables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CatalogoDelSe>().ToView("CatalogoDelSe");
            modelBuilder.Entity<RegistroDeVariable>().ToTable("Var_Variable");
        }

    }
}


