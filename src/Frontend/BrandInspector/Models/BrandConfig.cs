using System.Collections.Generic;

namespace BrandInspector.Models
{
    public class BrandConfig
    {
        public IList<string> Colors { get; set; }
        public IList<string> Fonts { get; set; }
        public IList<double> SizesPt { get; set; }

    }
}
