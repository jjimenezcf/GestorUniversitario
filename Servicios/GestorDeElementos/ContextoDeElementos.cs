using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using System.Linq;

namespace Gestor.Elementos
{
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
            DatosDeConexion.Version = new ExisteTabla(this, Literal.Tabla.Variable).Existe ?
             ObtenerVersion() :
             "0.0.0.";
            DatosDeConexion.Usuario = Literal.usuario;
        }

        private string ObtenerVersion()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.version);
            return registro == null ? "0.0.0" : registro.Valor;
        }

        public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }
        public DbSet<RegistroDeVariable> Variables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoDelSe>().ToView(Literal.Vista.Catalogo);
            modelBuilder.Entity<RegistroDeVariable>().ToTable(Literal.Tabla.Variable);
        }

    }
}


