using BrandInspector.ViewModels;
using System.Threading.Tasks;

namespace BrandInspector.Presenters.Interfaces
{
    public interface IMainPresenter
    {
        void OnFileSelected(string path);
        Task<ResultViewModel> ScanFonts();
        Task<ResultViewModel> ScanColors();
        Task<ResultViewModel> ScanSize();
        void CancelProcess();
    }
}
