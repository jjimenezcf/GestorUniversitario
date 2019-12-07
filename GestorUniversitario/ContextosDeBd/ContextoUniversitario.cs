using Gestor.Elementos.Universitario.ModeloBd;
using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;

namespace Gestor.Elementos.Universitario.ContextosDeBd
{

    public class ContextoUniversitario : ContextoDeElementos
    {
        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) :
        base(options)
        {
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Version = new VersionSql(this).Version;
            DatosDeConexion.Usuario = "jjimenezcf@gmail.com";
        }

        public DbSet<RegistroDeCurso> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }
        public DbSet<RegistroDeEstudiante> Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistroDeCurso>().ToTable("Curso");
            modelBuilder.Entity<RegistroDeInscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<RegistroDeEstudiante>().ToTable("Estudiante");

            modelBuilder.Entity<CatalogoDelSe>().ToView("CatalogoDelSe");
            modelBuilder.Ignore<ConsultaSql>();
        }

    }
}
