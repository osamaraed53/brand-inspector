using BrandInspector.ViewModels;
using System.Threading.Tasks;

namespace BrandInspector.Presenters.Interfaces
{
    public interface IMainPresenter
    {
        bool ValidateFileOnSelected(string path);
        Task<ResultViewModel> ScanFonts();
        Task<ResultViewModel> ScanColors();
        Task<ResultViewModel> ScanSize();
        void CancelProcess();
    }
}
