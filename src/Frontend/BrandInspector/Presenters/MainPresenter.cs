using BrandInspector.Constants;
using BrandInspector.Models;
using BrandInspector.Models.Enums;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.Services.Interfaces;
using BrandInspector.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrandInspector.Presenters
{
    public class MainPresenter : IMainPresenter
    {
        private CancellationTokenSource _currentCancellationToken;

        public readonly IMainView _view;
        public readonly IScannerService _scanner;
        public readonly IBrandClientService _brandClient;
        private readonly AppContext _appContext;


        public MainPresenter(MainForm mainView, IScannerService scannerService, IBrandClientService brandClientService, AppContext appContext)
        {
            _brandClient = brandClientService;
            _scanner = scannerService;
            _view = mainView;
            _appContext = appContext;
        }
        public void OnFileSelected(string path)
        {
            if (string.IsNullOrWhiteSpace(path) ||
                Path.GetExtension(path).ToLower() != ".pptx")
            {
                _view.ShowMessage(ErrorMessages.InvalidFile);
                return;
            }
            // TODO : check magic bytes 
        }

        public async Task<IList<ComplianceError>> ScanFonts()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _view.SelectedFilePath;
                var errors = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return ValidateFontsCompliance(errors);
            }
            catch (OperationCanceledException)
            {
                return new List<ComplianceError>();
            }
            finally
            {
                _currentCancellationToken = null;
            }
        }

        public async Task<IList<ComplianceError>> ScanColors()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _view.SelectedFilePath;
                var errors = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return ValidateColorCompliance(errors);
            }
            catch (OperationCanceledException)
            {
                return new List<ComplianceError>();
            }
            finally
            {
                _currentCancellationToken = null;
            }

        }

        public void CancelProcess()
        {
            _currentCancellationToken?.Cancel();
        }

        

        public async Task<IList<ComplianceError>> ScanSize()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _view.SelectedFilePath;
                var errors = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return ValidateFontSizeCompliance(errors);
            }
            catch (OperationCanceledException)
            {
                return new List<ComplianceError>();
            }
            finally
            {
                _currentCancellationToken = null;
            }

        }

        //TODO : separate this logic in other service if i can based on principle   
        private List<ComplianceError> ValidateFontsCompliance(List<TextRunInfo> textRunInfos)
        {
            var errors = new List<ComplianceError>();
            var fonts = _brandClient.GetBrandFonts();


            errors = textRunInfos.Where(obj => !fonts.Contains(obj.FontFamily, StringComparer.OrdinalIgnoreCase)).Select(obj => new ComplianceError
            {
                SlideNumber = obj.SlideNumber,
                ShapeType = obj.ShapeType,
                ShapeId = obj.ShapeId,
                IssueType = IssueTypes.Font,
                SampleText = obj.SampleText,
                Actual = obj.FontFamily,
                Expected = string.Join(", ", fonts),
                Severity = "warn"
            }).ToList();



            return errors;
        }

        private List<ComplianceError> ValidateFontSizeCompliance(List<TextRunInfo> textRunInfos)
        {
            var errors = new List<ComplianceError>();
            var sizes = _brandClient.GetBrandSizes(); ;

            errors = textRunInfos.Where(obj => !sizes.Any(size => Math.Abs(size - obj.FontSizePt) <= Constraints.SizeTolerance)).Select(obj => new ComplianceError
            {
                SlideNumber = obj.SlideNumber,
                ShapeType = obj.ShapeType,
                ShapeId = obj.ShapeId,
                IssueType = IssueTypes.Size,
                SampleText = obj.SampleText,
                Actual = obj.FontSizePt.ToString(),
                Expected = string.Join(", ", sizes.Select(s => s.ToString())),
                Severity = "warn"
            }).ToList();

            return errors;
        }


        private List<ComplianceError> ValidateColorCompliance(List<TextRunInfo> textRunInfos)
        {
            var errors = new List<ComplianceError>();
            var colors = _brandClient.GetBrandColors();
           

            errors = textRunInfos.Where(obj => !colors.Contains(obj.ColorHex, StringComparer.OrdinalIgnoreCase)).Select(obj => new ComplianceError
            {
                SlideNumber = obj.SlideNumber,
                ShapeType = obj.ShapeType,
                ShapeId = obj.ShapeId,
                IssueType = IssueTypes.Color,
                SampleText = obj.SampleText,
                Actual = obj.ColorHex,
                Expected = string.Join(", ", colors),
                Severity = "warn"
            }).ToList();

            return errors;
        }






    }
}
//var x = 
