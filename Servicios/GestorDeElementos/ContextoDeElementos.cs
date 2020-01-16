using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using System.Linq;
using GestorDeElementos.Utilidades;
using Extensiones;

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

        public TrazaSql Traza { get; private set; }

        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();

            dbContextOptionsBuilder.AddInterceptors(new LogSql());

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

        public void IniciarTraza()
        {
            if (Traza == null)
                CrearTraza(NivelDeTraza.Siempre, @"c:\Temp\Trazas", $"traza_{DateTime.Now}.txt");
            else
            if (!Traza.EstaAbierta)
                Traza.Abrir(true);
        }

        public void CerrarTraza()
        {
            if (Traza != null)
            {
                if (!Traza.EstaAbierta)
                    Traza.Abrir(true);

                Traza.CerrarTraza("Conexión cerrada");
            }
        }

        private void CrearTraza(NivelDeTraza nivel, string ruta, string fichero)
        {
            Traza = new TrazaSql(nivel, ruta, fichero, $"Traza iniciada por {DatosDeConexion.Usuario}");
        }

        
    }
}


