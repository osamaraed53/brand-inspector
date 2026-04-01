using BrandInspector.Models;
using Microsoft.EntityFrameworkCore;
namespace BrandInspector.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Brand>()
            .OwnsOne(b => b.BrandConfig, b =>
            {
                b.ToJson();
            });

        base.OnModelCreating(builder);

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }


}
