using BrandInspector.Presenters;
using BrandInspector.Services.Interfaces;
using BrandInspector.Services;
using BrandInspector.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace BrandInspector
{
    internal static class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {

            var services = new ServiceCollection();

            // Services
            services.AddSingleton<INavigationService, NavigationService>();

            // Presenters
            services.AddTransient<LoginPresenter>();
            services.AddTransient<MainPresenter>();

            // Forms
            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();

            // Register services
            services.AddTransient<MainForm>();

            ServiceProvider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
            Application.Run(loginForm);
        }
    }
}
