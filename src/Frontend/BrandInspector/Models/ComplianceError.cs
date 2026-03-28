using BrandInspector.Models.Enums;

namespace BrandInspector.Models
{
    public class ComplianceError
    {

        public int SlideNumber { get; set; }

        public string ShapeId { get; set; }

        public string ShapeType { get; set; }

        public IssueTypes IssueType { get; set; }

        public string SampleText { get; set; }

        public string Actual { get; set; }

        public string Expected { get; set; }

        public string Severity { get; set; }

    }
}
