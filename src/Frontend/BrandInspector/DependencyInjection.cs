using BrandInspector.Presenters;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.Services;
using BrandInspector.Services.Interfaces;
using BrandInspector.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BrandInspector
{
    public static class DependencyInjection
    {
        public static ServiceProvider Configure()
        {
            var services = new ServiceCollection();

            RegisterServices(services);
            RegisterForms(services);
            RegisterPresenters(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBrandClientService, BrandClientService>();
            services.AddSingleton<AppContext>();
            services.AddScoped<IScannerService, ScannerService>();
            services.AddSingleton<ITokenService, TokenService>();

        }

        private static void RegisterPresenters(IServiceCollection services)
        {
            services.AddTransient<ILoginPresenter, LoginPresenter>();
            services.AddTransient<IMainPresenter, MainPresenter>();


        }

        private static void RegisterForms(IServiceCollection services)
        {
            services.AddTransient(sp =>
            {
                var form = new MainForm();
                var scannerService = sp.GetRequiredService<IScannerService>();
                var brandClientService = sp.GetRequiredService<IBrandClientService>();
                var appContext = sp.GetRequiredService<AppContext>();
                var presenter = new MainPresenter(form, scannerService, brandClientService, appContext);
                form.Presenter = presenter;              
                return form;
            });

            IServiceCollection serviceCollection = services.AddTransient(sp =>
            {
                var form = new LoginForm();
                var apiClient = sp.GetRequiredService<ApiClient>();
                var appContext = sp.GetRequiredService<AppContext>();
                var loginPresenter = new LoginPresenter(apiClient, form, appContext);
                form.Presenter = loginPresenter;
                return form;
            });

            services.AddTransient<IMainView>(sp => sp.GetRequiredService<MainForm>());


        }
    }
}
