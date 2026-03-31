using BrandInspector.Presenters;
using BrandInspector.Services.Interfaces;
using BrandInspector.Services;
using BrandInspector.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using BrandInspector.Presenters.Interfaces;

namespace BrandInspector
{
    internal static class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {

            ServiceProvider = DependencyInjection.Configure();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var form = ServiceProvider.GetRequiredService<MainForm>();
            Application.Run(form);
        }



    }

}
