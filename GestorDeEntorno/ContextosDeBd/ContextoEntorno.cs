using System.Linq;
using Gestor.Elementos;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

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

    public class VersionSql : ConsultaSql
    {
        public string Version => (string)Registros[0][3];

        public VersionSql(ContextoDeElementos contexto)
            : base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.version}'")
        {
            Ejecutar();
        }
    }

    public class DebugarSql : ConsultaSql
    {
        public bool DebugarSqls => (Registros.Count == 1 ? Registros[0][3].ToString() == "S" : false);

        public DebugarSql(ContextoDeElementos contexto)
        : base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.DebugarSqls}'")
        {
            Ejecutar();
        }
    }

    public class ContextoEntorno : ContextoDeElementos
    {

        public DbSet<Fun_Elemento> Funcionalidades { get; set; }
        public DbSet<Fun_Accion> Acciones { get; set; }
        public DbSet<Var_Elemento> Variables { get; set; } 

        public ContextoEntorno(DbContextOptions<ContextoEntorno> options) :
        base(options)
        {
            InicializarDatosEntorno();
        }

        public void InicializarDatosEntorno()
        {
            DatosDeConexion.Version = new ExisteTabla(this, Literal.Tabla.Variable).Existe ?
                   ObtenerVersion() :
                   "0.0.0.";
            Debuggar = HayQueDebuggar();
        }

        private bool HayQueDebuggar()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.DebugarSqls);
            return registro == null ? false : registro.Valor == "S";

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Var_Elemento>();
            modelBuilder.Entity<Fun_Accion>();
            modelBuilder.Entity<Fun_Elemento>();
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

        private string ObtenerVersion()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.version);
            return registro == null ? "0.0.0" : registro.Valor;
        }


    }
}
