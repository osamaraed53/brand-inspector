using BrandInspector.Presenters.Interfaces;
using BrandInspector.Services;
using BrandInspector.Services.Interfaces;
using BrandInspector.Views;
using System;
using System.Threading.Tasks;

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

        public async Task<string> Login(string username, string password)
        {
            var response =  await Task.Run(() =>_apiClient.LoginAsync( username, password ));

            if (response.Item1)
            {
                _appContext.ShowForm<MainForm>(_loginView);
                return string.Empty;   
            }

            return response.Item2 ?? string.Empty;
        }
    }
}
