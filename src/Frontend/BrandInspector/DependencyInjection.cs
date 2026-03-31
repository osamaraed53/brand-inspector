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
            services.AddScoped<IScannerService, ScannerService>();
            services.AddSingleton<ITokenService, TokenService>();

        }

        private static void RegisterPresenters(IServiceCollection services)
        {
            services.AddTransient<ILoginPresenter, LoginPresenter>();

        }

        private static void RegisterForms(IServiceCollection services)
        {
            services.AddTransient<MainForm>(sp =>
            {
                var form = new MainForm();
                var scannerService = sp.GetRequiredService<IScannerService>();
                var brandClientService = sp.GetRequiredService<IBrandClientService>();
                var presenter = new MainPresenter(form,scannerService, brandClientService); 
                form.Presenter = presenter;              
                return form;
            });

            services.AddTransient<IMainView>(sp => sp.GetRequiredService<MainForm>());


        }
    }
}
