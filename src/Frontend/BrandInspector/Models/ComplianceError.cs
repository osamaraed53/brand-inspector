using BrandInspector.Models.Enums;

namespace BrandInspector.Models
{
    public class ComplianceError
    {

        public int SlideNumber { get; set; }

        public int  ShapId {  get; set; }

        public string ShapeType { get; set; }

        public IssueTypes IssueTypes { get; set; } 

        public string SampleText { get; set; }

        public string Actual {  get; set; }

        public string Expected { get; set; }

        public string Severity { get; set; }

    }
}
