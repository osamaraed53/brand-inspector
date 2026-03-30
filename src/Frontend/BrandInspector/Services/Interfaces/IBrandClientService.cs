using System.Collections;
using System.Collections.Generic;

namespace BrandInspector.Services.Interfaces
{
    public interface IBrandClientService
    {
        IList<string> GetBrandFonts();
        IList<double> GetBrandSizes();
        IList<string> GetBrandColors();

    }
}
