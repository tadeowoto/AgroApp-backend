using Microsoft.EntityFrameworkCore;


namespace api.agroapp.model
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

    }

}




