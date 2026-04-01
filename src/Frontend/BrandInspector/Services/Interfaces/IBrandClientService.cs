using System.Collections;
using System.Collections.Generic;
using static BrandInspector.Services.BrandClientService;
using System.Threading.Tasks;

namespace BrandInspector.Services.Interfaces
{
    public interface IBrandClientService
    {
        Task<IList<string>> GetBrandFonts();
        Task<IList<double>> GetBrandSizes();
        Task<IList<string>> GetBrandColors();

    }
}
