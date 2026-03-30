using System.Drawing;

namespace BrandInspector.Models
{
    public class TextRunInfo
    {

        public int SlideNumber { get; set; }
        // Note I'm add this 
        public string ShapeId { get; set; }
        public string ShapeType { get; set; }

        public string SampleText {  get; set; }
        
        public string FontFamily { get; set; }

        public double FontSizePt { get; set; }

        public string ColorHex { get; set; }


    }
}
