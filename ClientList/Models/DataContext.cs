using Microsoft.EntityFrameworkCore;

namespace ClientList.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {}

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
