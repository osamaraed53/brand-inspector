using BrandInspector.Models;

namespace BrandInspector.Services;

public interface IBrandConfigService
{
    Task CreateBrandData(Brand brand);
    Task<IEnumerable<string>> GetBrandFonts(CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetBrandColors(CancellationToken cancellationToken);
    Task<IEnumerable<double>> GetBrandSizes(CancellationToken cancellationToken);
}
