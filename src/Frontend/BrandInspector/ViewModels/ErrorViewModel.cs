using System.ComponentModel;

namespace BrandInspector.ViewModels
{
    public class ErrorViewModel
    {
        [DisplayName("Slide #")]
        public int SlideNumber { get; set; }
        [DisplayName("Shape Type")]
        public string ShapeType { get; set; }
        [DisplayName("Sample Text")]
        public string SampleText { get; set; }
        [DisplayName("Font  Family")]

        public string FontFamily { get; set; }
        [DisplayName("Font Size")]
        public double FontSizePt { get; set; }
        [DisplayName("Text Color")]

        public string ColorHex { get; set; }
        [DisplayName("Compliance")]
        public string Compliance { get; set; }

        //Slide #, Shape Type, Sample Text (first 30 chars), Font  Family, Font Size(pt), Text Color(#RRGGBB), Compliance (Pass/Fail + reason).
    }
}
