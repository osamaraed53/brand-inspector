using BrandInspector.Services.Interfaces;
using System.Collections.Generic;

namespace BrandInspector.Services
{
    public class BrandClientService : IBrandClientService
    {
        public IList<string> GetBrandColors()
        {
            return new List<string> { "#FF0000", "#00FF00", "#0000FF" };

        }

        public IList<string> GetBrandFonts()
        {
            return new List<string> { "Arial", "Haettenschweiler", "Tahoma" };
        }

        public IList<double> GetBrandSizes()
        {
            return new List<double> { 16, 18, 20, 28, 24, 40  };
        }


    }
}
