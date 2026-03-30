using BrandInspector.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandInspector.Data;

public interface IApplicationDbContext
{
    #region  DbSet
    public DbSet<User> Users { get; }
    public DbSet<Brand> Brands { get; }
    #endregion
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
