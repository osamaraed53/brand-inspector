using BrandInspector.Constants;
using BrandInspector.Models;
using BrandInspector.Services.Interfaces;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Presentation = DocumentFormat.OpenXml.Presentation;

namespace BrandInspector.Services
{
    public class ScannerService : IScannerService
    {
        public bool IsOpenPPTXValid(string filePath)
        {
            try
            {
                using (var doc = PresentationDocument.Open(filePath, false))
                {
                    var validator = new OpenXmlValidator();
                    var errors = validator.Validate(doc);

                    return !errors.Any();
                }
            }
            catch
            {
                return false;
            }
        }
        public List<TextRunInfo> ScanPresentation(string filePath, CancellationToken token)
        {
            var textRunInfos = new List<TextRunInfo>();

            using (PresentationDocument presentationDocument = PresentationDocument.Open(filePath, false))
            {
                var presentationPart = presentationDocument.PresentationPart;

                var slides = presentationPart.SlideParts;


                var slidesMaster = presentationPart.SlideMasterParts;

                var themePart = presentationPart.ThemePart;
                var defaultColor = GetDefaultColorFromTheme(themePart);
                var defaultFont = GetDefaultFontFromTheme(themePart);

                foreach (var slidePart in slides)
                {

                    int slideNumber = presentationPart.SlideParts.ToList().IndexOf(slidePart) + 1;
                    var slide = slidePart.Slide;

                    var masterDefaults = GetMasterDefaults(slidePart, defaultColor, defaultFont);

                    ProcessTextShapes(slide, slideNumber, textRunInfos, masterDefaults, token );
                    ProcessTableTextRuns(slide, slideNumber, textRunInfos, masterDefaults, token);
                    token.ThrowIfCancellationRequested();

                }
            }

            return textRunInfos;
        }


        private void ProcessTextShapes(Slide slide, int slideNumber, List<TextRunInfo> textRunInfos, (string fontFamily, double fontSize, string colorHex) masterDefaults, CancellationToken token)
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

                    string fontFamily = GetFontFamily(runProperties) ?? masterDefaults.fontFamily;
                    double fontSize = GetFontSize(runProperties);
                    string colorHex = GetColorHex(runProperties)?? masterDefaults.colorHex;

                    if(fontSize == 0)
                        fontSize = masterDefaults.fontSize;

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
        private void ProcessTableTextRuns(Slide slide, int slideNumber, List<TextRunInfo> textRunInfos, (string fontFamily, double fontSize, string colorHex) masterDefaults, CancellationToken token)
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

                            string fontFamily = GetFontFamily(runProperties) ?? masterDefaults.fontFamily;
                            double fontSize = GetFontSize(runProperties);
                            string colorHex = GetColorHex(runProperties) ?? masterDefaults.colorHex;

                            if (fontSize == 0)
                                fontSize = masterDefaults.fontSize;


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

                return latinFont?.Typeface?.Value ?? eastAsianFont?.Typeface?.Value ?? complexScriptFont?.Typeface?.Value ?? string.Empty;
            }
            return null;
        }
        private string GetFontFamily(DefaultRunProperties runProperties)
        {
            if (runProperties != null)
            {
                var latinFont = runProperties.GetFirstChild<LatinFont>();
                var eastAsianFont = runProperties.GetFirstChild<EastAsianFont>();
                var complexScriptFont = runProperties.GetFirstChild<ComplexScriptFont>();

                return latinFont?.Typeface?.Value ?? eastAsianFont?.Typeface?.Value ?? complexScriptFont?.Typeface?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        private double GetFontSize(RunProperties runProperties)
        {
            if (runProperties != null && runProperties.FontSize != null)
                return runProperties.FontSize.Value / 100.0;

            return 0.0;
        }
        private double GetFontSize(DefaultRunProperties runProperties)
        {
            if (runProperties != null && runProperties.FontSize != null)
                return runProperties.FontSize.Value / 100.0;

            return 0.0;
        }


        private string GetColorHex(RunProperties runProperties)
        {
            if (runProperties?.GetFirstChild<SolidFill>()?.GetFirstChild<RgbColorModelHex>() is RgbColorModelHex rgbColor)
                return $"#{rgbColor.Val}";

            return null;
        }
        private string GetColorHex(DefaultRunProperties runProperties)
        {
            if (runProperties?.GetFirstChild<SolidFill>()?.GetFirstChild<RgbColorModelHex>() is RgbColorModelHex rgbColor)
                return $"#{rgbColor.Val}";

            return "#000000";
        }
        // TODO : check nullable in  GetFontFamily GetFontSize GetColorHex
        private (string font, double size, string color) GetMasterDefaults(SlidePart slidePart, string defaultColor, string defaultFont)
        {
            string font = defaultFont;
            double size = 0.0; // TODO : should use real default number 
            string color = defaultColor;


            var layoutPart = slidePart.SlideLayoutPart;
            var masterPart = layoutPart?.SlideMasterPart;

            //  from master body styleee
            if (masterPart?.SlideMaster?.TextStyles?.BodyStyle != null)
            {
                var bodyStyle = masterPart.SlideMaster.TextStyles.BodyStyle;
                var defaultRun = bodyStyle?.Descendants<DefaultRunProperties>().FirstOrDefault();

                if (defaultRun != null)
                {
                   font = GetFontFamily(defaultRun);
                   size = GetFontSize(defaultRun); 
                   color = GetColorHex(defaultRun);
                }
            }


            return (font, size, color);
        }
        private string GetDefaultColorFromTheme(ThemePart themePart)
        {

            var colorScheme = themePart?.Theme?.ThemeElements?.ColorScheme;
            var dark1 = colorScheme?.Dark1Color?.GetFirstChild<RgbColorModelHex>();
            if (dark1?.Val != null)
                return $"#{dark1.Val}";

            return "#000000";
        }
        private string GetDefaultFontFromTheme(ThemePart themePart)
        {

            var fontScheme = themePart?.Theme?.ThemeElements?.FontScheme;
            var majorFont = fontScheme?.MajorFont?.LatinFont?.Typeface;
            if (majorFont != null)
                return majorFont;

            var minorFont = fontScheme?.MinorFont?.LatinFont?.Typeface;
            if (minorFont != null)
                return minorFont;

            return string.Empty;
        }

    }
}

//var x = slidePart.Slide.OpenXmlElementContext;

//var reader = new StreamReader(slidePart.GetStream());

//var y =  reader.ReadToEnd();