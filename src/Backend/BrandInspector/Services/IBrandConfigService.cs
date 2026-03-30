using BrandInspector.Models;

namespace BrandInspector.Services;

public interface IBrandConfigService
{
    Task CreateBrandData(Brand brand);
    Task<IEnumerable<string>> GetBrandFonts();
    Task<IEnumerable<string>> GetBrandColors();
    Task<IEnumerable<double>> GetBrandSizes();
}
