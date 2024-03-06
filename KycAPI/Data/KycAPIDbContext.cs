using KycAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KycAPI.Data
{
    public class KycAPIDbContext : DbContext
    {
        public KycAPIDbContext(DbContextOptions options) : base(options)
        {
            Entities = Set<Entity>();
        }

        public DbSet<Entity> Entities { get; set; }
    }
}
