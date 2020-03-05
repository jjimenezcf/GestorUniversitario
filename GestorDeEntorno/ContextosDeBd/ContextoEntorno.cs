using System.Configuration;
using System.Linq;
using Gestor.Elementos;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Entorno
{
    class Literal
    {
        internal static readonly string DebugarSqls = nameof(DebugarSqls);
        internal static readonly string esquemaBd = "ENTORNO";
        internal static readonly string version = "Versión";

        internal class Tabla
        {
            internal static string Variable = "Var_Elemento";
        }
    }
       
    public class ContextoEntorno : ContextoDeElementos
    {

        public DbSet<Fun_Elemento> Funcionalidades { get; set; }
        public DbSet<Fun_Accion> Acciones { get; set; }
        public DbSet<Var_Elemento> Variables { get; set; }
        
        public ContextoEntorno(DbContextOptions<ContextoEntorno> options, IConfiguration configuracion) :
        base(options, configuracion)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Var_Elemento>();
            modelBuilder.Entity<Fun_Accion>();
            modelBuilder.Entity<Fun_Elemento>();
        }
        private bool HayQueDebuggar()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.DebugarSqls);
            return registro == null ? false : registro.Valor == "S";
        }

        private string ObtenerVersion()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.version);
            return registro == null ? "0.0.0" : registro.Valor;
        }

        public static void NuevaVersion(ContextoEntorno cnx)
        {
            var version = cnx.Variables.SingleOrDefault(v => v.Nombre == Literal.version);
            if (version == null)
            {
                cnx.Variables.Add(new Var_Elemento { Nombre = Literal.version, Descripcion = "Versión del producto", Valor = "0.0.1" });
            }
            else
            {
                version.Valor = "0.0.2";
                cnx.Variables.Update(version);
            }
            cnx.SaveChanges();
        }   


    }
}
