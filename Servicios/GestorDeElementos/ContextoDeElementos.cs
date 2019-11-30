using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;

namespace Gestor.Elementos
{
    public class ContextoDeElementos : DbContext
    {
        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {
        }

       public DbSet<RegistroDelCatalogoDelSe> CatalogoDelSe { get; set; }

    }
}


