using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Gestor.Elementos.Usuario
{

    public class ContextoUniversitario : ContextoDeElementos
    {
        public DbSet<RegistroDeCurso> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }
        public DbSet<UsuarioReg> Usuarios { get; set; }

        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegistroDeCurso>();
            modelBuilder.Entity<RegistroDeInscripcion>();
            modelBuilder.Entity<UsuarioReg>();

        }

        public static void InicializarMaestros(ContextoUniversitario contexto)
        {
            if (!contexto.Usuarios.Any())
                Maestros.CrearDatosIniciales(contexto);

        }

    }
}
