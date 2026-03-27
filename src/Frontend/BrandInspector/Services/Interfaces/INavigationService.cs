using System.Windows.Forms;

namespace BrandInspector.Services.Interfaces
{
    public interface INavigationService
    {
        void Show<TForm>(Form current) where TForm : Form;
        void ShowDialog<TForm>() where TForm : Form;


    }
}
