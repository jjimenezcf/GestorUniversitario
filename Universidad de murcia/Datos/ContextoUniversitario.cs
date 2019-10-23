using Microsoft.EntityFrameworkCore;
using UniversidadDeMurcia.Models;

namespace UniversidadDeMurcia.Datos
{


    public class ContextoUniversitario : DbContext
    {
        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>().ToTable("Curso");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
        }
    }
}
