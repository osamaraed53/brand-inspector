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
            RegisterPresenters(services);
            RegisterForms(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
        }

        private static void RegisterPresenters(IServiceCollection services)
        {
            services.AddTransient<ILoginPresenter, LoginPresenter>();
            services.AddTransient<IMainPresenter, MainPresenter>();
        }

        private static void RegisterForms(IServiceCollection services)
        {
            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();
        }
    }
}
