using GestorUniversitario.BdModelo;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GestorDeElementos;
using GestorDeElementos.BdModelo;


namespace GestorUniversitario.ContextosDeBd
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
            modelBuilder.Entity<RegistroDeCurso>().ToTable("Curso");
            modelBuilder.Entity<RegistroDeInscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<RegistroDeEstudiante>().ToTable("Estudiante");
            modelBuilder.Entity<RegistroDeCatalogoDeBd>().ToTable("TABLES", schema: "INFORMATION_SCHEMA");
        }

    }
}
