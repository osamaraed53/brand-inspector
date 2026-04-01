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
        [STAThread]
        static void Main()
        {

            var serviceProvider = DependencyInjection.Configure();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var loginForm = serviceProvider.GetRequiredService<LoginForm>();
            var appContext = serviceProvider.GetRequiredService<AppContext>();

            appContext.Show(loginForm);
            Application.Run(appContext);
            
        }



    }

}
