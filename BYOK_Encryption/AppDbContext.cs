using BYOK_Encryption.Entity;
using Microsoft.EntityFrameworkCore;

namespace BYOK_Encryption
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
           : base(options) { }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<BYOK_Enabled_Tenants> bYOK_Enabled_Tenants { get; set; }
    }
}
