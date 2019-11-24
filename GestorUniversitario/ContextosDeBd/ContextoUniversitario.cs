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

        public DbSet<BdCurso> Cursos { get; set; }
        public DbSet<BdInscripcion> Inscripciones { get; set; }
        public DbSet<BdEstudiante> Estudiantes { get; set; }


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
            modelBuilder.Entity<BdCurso>().ToTable("Curso");
            modelBuilder.Entity<BdInscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<BdEstudiante>().ToTable("Estudiante");
            modelBuilder.Entity<BdCatalogoDeBd>().ToTable("TABLES", schema: "INFORMATION_SCHEMA");
        }

    }
}
