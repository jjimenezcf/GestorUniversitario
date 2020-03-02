using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Gestor.Elementos.Universitario
{

    public class ContextoUniversitario : ContextoDeElementos
    {
        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) :
        base(options)
        {

        }

        public DbSet<RegistroDeCurso> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }
        public DbSet<RegistroDeEstudiante> Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegistroDeCurso>().ToTable("Curso");
            modelBuilder.Entity<RegistroDeInscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<RegistroDeEstudiante>().ToTable("Estudiante");

        }

        public static void InicializarMaestros(ContextoUniversitario contexto)
        {
            if (!contexto.Estudiantes.Any())
                Maestros.CrearDatosIniciales(contexto);

        }

    }
}
