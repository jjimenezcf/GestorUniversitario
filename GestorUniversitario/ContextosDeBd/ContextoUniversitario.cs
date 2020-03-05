using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Gestor.Elementos.Universitario
{

    public class ContextoUniversitario : ContextoDeElementos
    {
        public DbSet<RegistroDeCurso> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }
        public DbSet<Usuario> Estudiantes { get; set; }

        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegistroDeCurso>();
            modelBuilder.Entity<RegistroDeInscripcion>();
            modelBuilder.Entity<Usuario>();

        }

        public static void InicializarMaestros(ContextoUniversitario contexto)
        {
            if (!contexto.Estudiantes.Any())
                Maestros.CrearDatosIniciales(contexto);

        }

    }
}
