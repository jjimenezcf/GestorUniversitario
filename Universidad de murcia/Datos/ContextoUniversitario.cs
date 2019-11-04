using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public IQueryable<T> Elementos<T>() => default(T) switch {
            Curso _ => (IQueryable<T>) Cursos,
            Inscripcion _ => (IQueryable<T>) Inscripciones,
            Estudiante _ => (IQueryable<T>) Estudiantes,
            _ => default
        };


        //public string kk<T>(T o) => o switch
        //{
        //    string _ => $"Es una cadena",
        //    int entero => $"Es un entero: más 2 = {entero + 2}",
        //    Estudiante est => $"Es un estudiante: {est.Nombre}",
        //    Curso cur => $"Es un curso: {cur.Titulo}",
        //    _ => "es otra cosa"
        //};


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>().ToTable("Curso");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
        }
    }
}
