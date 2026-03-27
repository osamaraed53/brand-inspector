using BrandInspector.Services.Interfaces;
using BrandInspector.Views;

namespace BrandInspector.Presenters
{
    public class LoginPresenter
    {
        private readonly INavigationService _navigation;
        public LoginPresenter(INavigationService navigation)
        {
            _navigation = navigation;
        }


    }
}
