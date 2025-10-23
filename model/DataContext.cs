using api_agroapp.model;
using Microsoft.EntityFrameworkCore;


namespace api.agroapp.model
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<Campo> Campo { get; set; }
        public DbSet<Recurso> Recurso { get; set; }
        public DbSet<Cosecha> Cosecha { get; set; }
        public DbSet<TipoActividad> TipoActividad { get; set; }

    }

}




