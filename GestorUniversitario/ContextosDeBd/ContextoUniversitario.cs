using Microsoft.EntityFrameworkCore;
using System.Linq;
using GestorUniversitario.ModeloDeClases;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using Gestor.Errores;

namespace GestorUniversitario.ContextosDeBd
{

    public class GestorDeElementos: DbContext
    {
        public GestorDeElementos(DbContextOptions<ContextoUniversitario> options) :
        base(options)
        { 
        }
        public EntityEntry Crear(object nuevo)
        {
            EntityEntry nuevoElemento = null;
            try
            {
                antesDeCrear(nuevo);
                nuevoElemento = base.Add(nuevo);
                despuesDeCrear(nuevoElemento);
            }
            catch (Exception e)
            {
                var mensajeError = Errores.Concatenar(e);
                var nueva = new Exception("Error al crear el elemento");
                nueva.Data["ErrorOriginal"] = mensajeError;
                throw nueva;
            }
            finally
            {
                trasCrear(nuevoElemento);
            }

            return nuevoElemento;
        }


        private void antesDeCrear(object nuevo)
        {
        }

        private void despuesDeCrear(EntityEntry elemento)
        {
        }

        private void trasCrear(EntityEntry nuevoElemento)
        {
            throw new NotImplementedException();
        }

        public static async Task Crear(GestorDeElementos gestor,  Elemento elemento)
        {
            gestor.Crear(elemento);
            await gestor.SaveChangesAsync();
        }
    }

    public class ContextoUniversitario : GestorDeElementos
    {
        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) :
        base(options)
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
