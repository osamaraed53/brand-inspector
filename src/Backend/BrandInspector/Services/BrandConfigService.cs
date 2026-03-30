using BrandInspector.Constants;
using BrandInspector.Data;
using BrandInspector.Exceptions;
using BrandInspector.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandInspector.Services;
public class BrandConfigService(IApplicationDbContext context) : IBrandConfigService
 {

    public async Task CreateBrandData(Brand brand)
    {
        context.Brands.Add(new Brand
        {
            Name = "DefaultBrand",
            BrandConfig = new BrandConfig
            {
                Fonts = ["Arial", "Helvetica Neue", "Tahoma"],
                Colors = ["#111827", "#5A1EFF", "#FFFFFF"],
                Sizes = [10, 12, 14, 16, 20, 24]
            }
        });

       await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetBrandFonts() 
    {
        var brand = await context.Brands.FirstOrDefaultAsync();

        if (brand == null) throw new NotFoundException(ErrorMessages.NotFound);

        return brand.BrandConfig.Fonts ?? [];
    }

    public async Task<IEnumerable<string>> GetBrandColors()
    {

        var brand = await context.Brands.FirstOrDefaultAsync();

        if (brand == null) throw new NotFoundException(ErrorMessages.NotFound);

        return brand.BrandConfig.Colors ?? [];
    }

    public async Task<IEnumerable<double>> GetBrandSizes()
    {

        var brand = await context.Brands.FirstOrDefaultAsync();

        if (brand == null) throw new NotFoundException(ErrorMessages.NotFound);

        return brand.BrandConfig.Sizes ?? [];
    }
}
