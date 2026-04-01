using BrandInspector.Services.Interfaces;
using BrandInspector.Views;

namespace BrandInspector.Presenters
{
    public class LoginPresenter : ILoginPresenter
    {
        private readonly AppContext _appContext;
        private readonly LoginForm _loginView;
        private readonly ApiClient _apiClient;
        public LoginPresenter( ApiClient apiClient ,LoginForm loginForm  , AppContext appContext)
        {
            _appContext = appContext;
            _apiClient = apiClient;
            _loginView = loginForm;
        }


    }
}
