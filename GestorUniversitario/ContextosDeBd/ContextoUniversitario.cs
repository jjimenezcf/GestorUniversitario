using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Gestor.Elementos.Universitario
{

    public class ContextoUniversitario : ContextoDeElementos
    {
        public DbSet<RegistroDeCurso> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }
        public DbSet<RegistroDeEstudiante> Estudiantes { get; set; }

        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) :
        base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegistroDeCurso>();
            modelBuilder.Entity<RegistroDeInscripcion>();
            modelBuilder.Entity<RegistroDeEstudiante>();

        }

        public static void InicializarMaestros(ContextoUniversitario contexto)
        {
            if (!contexto.Estudiantes.Any())
                Maestros.CrearDatosIniciales(contexto);

        }

    }
}
