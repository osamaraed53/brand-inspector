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

                    

                    ProcessTextShapes(slide, slideNumber, textRunInfos,token);
                    ProcessTableTextRuns(slide, slideNumber, textRunInfos, token);
                    token.ThrowIfCancellationRequested();

                }
            }

            return textRunInfos;
        }

        private void ProcessTextShapes(Slide slide, int slideNumber, List<TextRunInfo> textRunInfos, CancellationToken token)
        {
            var textElements = slide.Descendants<Presentation.Shape>().Where(s => s.TextBody != null).ToList();

            

            foreach (var shape in textElements)
            {
                string shapeId = shape.NonVisualShapeProperties.NonVisualDrawingProperties.Id;
                string shapeType = shape.NonVisualShapeProperties.NonVisualDrawingProperties.Name;
                var textBody = shape.TextBody;
                var textRuns = textBody.Descendants<Run>();

                foreach (var textRun in textRuns)
                {
                    token.ThrowIfCancellationRequested();
                    string sampleText = textRun.InnerText.Length > Constraints.LenOfSampleText ? textRun.InnerText.Substring(0, Constraints.LenOfSampleText) : textRun.InnerText;
                    var runProperties = textRun.RunProperties;

                    string fontFamily = GetFontFamily(runProperties);
                    double fontSize = GetFontSize(runProperties);
                    string colorHex = GetColorHex(runProperties);

                    var textRunInfo = new TextRunInfo
                    {
                        SlideNumber = slideNumber,
                        ShapeType = shapeType,
                        ShapeId = shapeId,
                        SampleText = sampleText,
                        FontFamily = fontFamily,
                        FontSizePt = fontSize,
                        ColorHex = colorHex
                    };
                    textRunInfos.Add(textRunInfo);
                }
            }
        }
        private void ProcessTableTextRuns(Slide slide,int slideNumber,List<TextRunInfo> textRunInfos,CancellationToken token)
        {
            foreach (var table in slide.Descendants<Table>())
            {
                foreach (var cell in table.Descendants<TableCell>())
                {
                    if (cell.TextBody == null) continue;

                    foreach (var para in cell.TextBody.Descendants<Paragraph>())
                    {
                        foreach (var run in para.Descendants<Run>())
                        {
                            token.ThrowIfCancellationRequested();

                            string text = run.Text?.Text ?? string.Empty;

                            string sampleText = text.Length > Constraints.LenOfSampleText
                                ? text.Substring(0, Constraints.LenOfSampleText)
                                : text;

                            var runProperties = run.RunProperties;

                            string fontFamily = GetFontFamily(runProperties);
                            double fontSize = GetFontSize(runProperties);
                            string colorHex = GetColorHex(runProperties);

                            var textRunInfo = new TextRunInfo
                            {
                                SlideNumber = slideNumber,
                                ShapeType = "Table cell",
                                ShapeId = "1", 
                                SampleText = sampleText,
                                FontFamily = fontFamily,
                                FontSizePt = fontSize,
                                ColorHex = colorHex
                            };

                            textRunInfos.Add(textRunInfo);
                        }
                    }
                }
            }
        }
        private string GetFontFamily(RunProperties runProperties)
        {
            if (runProperties != null)
            {
                var latinFont = runProperties.GetFirstChild<LatinFont>();
                var eastAsianFont = runProperties.GetFirstChild<EastAsianFont>();
                var complexScriptFont = runProperties.GetFirstChild<ComplexScriptFont>();

                return latinFont?.Typeface?.Value ?? eastAsianFont?.Typeface?.Value ?? complexScriptFont?.Typeface?.Value ??  string.Empty;
            }
            return string.Empty;
        }

        private double GetFontSize(RunProperties runProperties)
        {
            if (runProperties != null && runProperties.FontSize != null)
                return runProperties.FontSize.Value / 100.0;
            
            return 0.0;
        }

        private string GetColorHex(Drawing.RunProperties runProperties)
        {
            if (runProperties?.GetFirstChild<SolidFill>()?.GetFirstChild<RgbColorModelHex>() is RgbColorModelHex rgbColor)
                return $"#{rgbColor.Val}";
            
            return "#000000";
        }



    }
}

