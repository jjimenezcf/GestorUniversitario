using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;

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
        }

       public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }
       public DbSet<ConsultaSql> ConsultarValor { get; set; }

    }
}


