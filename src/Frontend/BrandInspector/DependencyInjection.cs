using BrandInspector.Presenters;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.Services;
using BrandInspector.Services.Interfaces;
using BrandInspector.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

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
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<HttpClient>();

            services.AddSingleton<AppContext>();

            //TODO : find better way to save uri
            services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7238")
            });

            services.AddScoped<IBrandClientService, BrandClientService>();
            services.AddScoped<IScannerService, ScannerService>();

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
        }
    }
}
