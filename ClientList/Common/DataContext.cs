using Microsoft.EntityFrameworkCore;
using ClientList.Features.Client.Models;
using ClientList.Features.User.Models;

namespace ClientList.Common.Data
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
