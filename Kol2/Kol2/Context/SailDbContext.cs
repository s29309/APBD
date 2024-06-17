using Kol2.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kol2.Context;

public class SailDbContext : DbContext
{
    protected SailDbContext() {}
    
    public SailDbContext(DbContextOptions options) : base(options) {}
    
    public DbSet<Client> Clients { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SailDbContext).Assembly); 
    }
}