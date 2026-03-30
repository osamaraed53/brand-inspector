using BrandInspector.Constants;
using BrandInspector.Models;
using BrandInspector.Services.Interfaces;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Drawing = DocumentFormat.OpenXml.Drawing;
using Presentation = DocumentFormat.OpenXml.Presentation;

namespace BrandInspector.Services
{
    public class ScannerService  : IScannerService
    {

        public  List<TextRunInfo> ScanPresentation(string filePath, CancellationToken token)
        {
            var textRunInfos = new List<TextRunInfo>();

            using (PresentationDocument presentationDocument = PresentationDocument.Open(filePath, false))
            {
                var presentationPart = presentationDocument.PresentationPart;

                var slides = presentationPart.SlideParts;
                
                foreach (var slidePart in slides)
                {


                    //var x = slidePart.Slide.OuterXml;

                    int slideNumber = presentationPart.SlideParts.ToList().IndexOf(slidePart) + 1;
                    var slide = slidePart.Slide;

                    

                    //ProcessTextShapes(slide, slideNumber, textRunInfos,token);
                    //ProcessTableTextRuns(slide, slideNumber, textRunInfos, token);
                    token.ThrowIfCancellationRequested();

                }
            }

            return textRunInfos;
        }
    {
    }
}
    }
}

