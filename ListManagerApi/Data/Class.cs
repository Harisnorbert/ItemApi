using Microsoft.EntityFrameworkCore;
using ListManagerApi.Models;

namespace ListManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //Item tábla
        public DbSet<Item> Items { get; set; }
    }
}
