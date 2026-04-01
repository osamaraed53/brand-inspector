using BrandInspector.Constants;
using BrandInspector.Models;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.Services.Interfaces;
using BrandInspector.ViewModels;
using BrandInspector.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrandInspector.Presenters
{
    public class MainPresenter : IMainPresenter
    {
        private CancellationTokenSource _currentCancellationToken;
        public readonly MainForm _mainView;
        public readonly IScannerService _scanner;
        public readonly IBrandClientService _brandClient;
        private readonly AppContext _appContext;


        public MainPresenter(MainForm mainView, IScannerService scannerService, IBrandClientService brandClientService, AppContext appContext)
        {

            _brandClient = brandClientService;
            _scanner = scannerService;
            _mainView = mainView;
            _appContext = appContext;
        }
        public bool ValidateFileOnSelected(string path)
        {
            if (string.IsNullOrWhiteSpace(path) ||
                System.IO.Path.GetExtension(path).ToLower() != ".pptx" || !_scanner.IsOpenPPTXValid(path))
            {
                _mainView.SelectedFilePath = string.Empty;
                _mainView.ShowMessage(ErrorMessages.InvalidFile);
                return false;
            }
            return true;
        }

        public async Task<ResultViewModel> ScanFonts()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _mainView.SelectedFilePath;
                var data = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return new ResultViewModel()
                {
                    Total = data.Count,
                    Errors = await Task.Run(() => ValidateFontsCompliance(data))
                };
            }
            catch (OperationCanceledException)
            {
                return new ResultViewModel() { Total = 0, Errors = new List<ErrorViewModel>() };
            }
            catch (UnauthorizedAccessException)
            {
                _appContext.ShowForm<LoginForm>(_mainView);
                return null; 
            }
            finally
            {
                _currentCancellationToken = null;
            }
        }

        public async Task<ResultViewModel> ScanColors()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _mainView.SelectedFilePath;
                var data = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return new ResultViewModel()
                {
                    Total = data.Count,
                    Errors = await Task.Run(() =>  ValidateColorCompliance(data))
                };
            }
            catch (OperationCanceledException)
            {
                return new ResultViewModel() { Total = 0, Errors = new List<ErrorViewModel>() };
            }
            catch (UnauthorizedAccessException)
            {
                _appContext.ShowForm<LoginForm>(_mainView);
                return null;
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



        public async Task<ResultViewModel> ScanSize()
        {
            _currentCancellationToken = new CancellationTokenSource();
            var token = _currentCancellationToken.Token;

            try
            {
                string filePath = _mainView.SelectedFilePath;
                var data = await Task.Run(() => _scanner.ScanPresentation(filePath, token));

                return new ResultViewModel()
                {
                    Total = data.Count,
                    Errors = await Task.Run(() => ValidateFontSizeCompliance(data))
                };
            }

            catch (OperationCanceledException)
            {
                return new ResultViewModel() { Total = 0, Errors = new List<ErrorViewModel>() };
            }
            catch (UnauthorizedAccessException)
            {
                _appContext.ShowForm<LoginForm>(_mainView);
                return null;
            }
            finally
            {
                _currentCancellationToken = null;
            }

        }

        //TODO : separate this logic in other service if i can based on principle   
        private async Task<List<ErrorViewModel>> ValidateFontsCompliance(List<TextRunInfo> textRunInfos)
        {
            var fonts = await _brandClient.GetBrandFonts();

            var fontSet = new HashSet<string>(fonts, StringComparer.OrdinalIgnoreCase);
            string fontsExpected = string.Join(", ", fontSet);

            var errors = new List<ErrorViewModel>();
            int counter = 1;

            foreach (var obj in textRunInfos)
            {
                if (!fontSet.Contains(obj.FontFamily))
                {
                    errors.Add(new ErrorViewModel
                    {
                        RowIndex = counter++,
                        SlideNumber = obj.SlideNumber,
                        ShapeType = obj.ShapeType,
                        SampleText = obj.SampleText,
                        FontFamily = obj.FontFamily,
                        FontSizePt = obj.FontSizePt,
                        ColorHex = obj.ColorHex,
                        Compliance = $"Fail expected: {fontsExpected}."
                    });
                }
            }

            return errors;
        }
        private async Task<List<ErrorViewModel>> ValidateFontSizeCompliance(List<TextRunInfo> textRunInfos)
        {
            var errors = new List<ErrorViewModel>();

            var sizes = await _brandClient.GetBrandSizes(); ;
            string sizesExpected = string.Join(", ", sizes);
            int counter = 1;

            foreach (var obj in textRunInfos)
            {
                if (!sizes.Any(size => Math.Abs(size - obj.FontSizePt) <= Constraints.SizeTolerance))
                {
                    errors.Add(new ErrorViewModel()
                    {
                        RowIndex = counter++,
                        SlideNumber = obj.SlideNumber,
                        ShapeType = obj.ShapeType,
                        SampleText = obj.SampleText,
                        FontFamily = obj.FontFamily,
                        FontSizePt = obj.FontSizePt,
                        ColorHex = obj.ColorHex,
                        Compliance = $"Fail expected: {sizesExpected}."
                    });

                }
            }
            return errors;
        }


        private async Task<List<ErrorViewModel>> ValidateColorCompliance(List<TextRunInfo> textRunInfos)
        {
            var errors = new List<ErrorViewModel>();
            var colors = await _brandClient.GetBrandColors();
            var colorsSet = new HashSet<string>(colors, StringComparer.OrdinalIgnoreCase);

            string colorExpected = string.Join(", ", colors);
            int counter = 1;
            foreach (var obj in textRunInfos)
            {
                if (!colorsSet.Contains(obj.ColorHex, StringComparer.OrdinalIgnoreCase))
                {
                    errors.Add(new ErrorViewModel()
                    {
                        RowIndex = counter++,
                        SlideNumber = obj.SlideNumber,
                        ShapeType = obj.ShapeType,
                        SampleText = obj.SampleText,
                        FontFamily = obj.FontFamily,
                        FontSizePt = obj.FontSizePt,
                        ColorHex = obj.ColorHex,
                        Compliance = $"Fail expected: {colorExpected}."
                    });
                }
            }
            return errors;
        }

    }
}

