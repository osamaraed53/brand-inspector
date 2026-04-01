using System.Threading.Tasks;

namespace BrandInspector.Presenters.Interfaces
{
    public interface ILoginPresenter
    {
        Task<string> Login(string username, string password);

    }
}
