using System.Drawing;

namespace BrandInspector.Models
{
    public class TextRunInfo
    {

        public string SlideNumber { get; set; }
        
        public string ShapeType { get; set; }

        public string SampleText {  get; set; }
        
        public string FontFamily { get; set; }

        public double FontSizePt { get; set; }

        public double ColorHex { get; set; }


        //`TextRunInfo`: SlideNumber, ShapeType, SampleText, FontFamily, FontSizePt, ColorHex.
    }
}
