using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Gestor.Elementos.Permiso
{

    public class CtoPermisos : ContextoDeElementos
    {
        public DbSet<PermisoReg> Cursos { get; set; }
        public DbSet<RegistroDeInscripcion> Inscripciones { get; set; }

        public CtoPermisos(DbContextOptions<CtoPermisos> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PermisoReg>();
            modelBuilder.Entity<RegistroDeInscripcion>();

        }


    }
}
