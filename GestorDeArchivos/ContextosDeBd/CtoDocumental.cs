using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Archivos
{
    public class ConstructorDelContexto : IDesignTimeDbContextFactory<CtoDocumental>
    {
        public CtoDocumental CreateDbContext(string[] arg)
        {

            var datosDeConexion = ContextoDeElementos.ObtenerDatosDeConexion();

            var opciones = new DbContextOptionsBuilder<CtoDocumental>();
            opciones.UseSqlServer(datosDeConexion.CadenaConexion);
            object[] parametros = { opciones.Options, datosDeConexion.Configuracion };

            return (CtoDocumental)Activator.CreateInstance(typeof(CtoDocumental), parametros);
        }
    }

    public class CtoDocumental : ContextoDeElementos
    {
        public static CtoDocumental CrearContexto()
        {
            return (CtoDocumental)ObtenerContexto(nameof(CtoDocumental), () => new ConstructorDelContexto().CreateDbContext(new string[] { }));
        }


        #region dbSets del contexto de seguridad

        #endregion

        public CtoDocumental(DbContextOptions<CtoDocumental> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
