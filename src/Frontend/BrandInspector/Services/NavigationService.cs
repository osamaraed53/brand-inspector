using BrandInspector.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace BrandInspector.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show<TForm>(Form current) where TForm : Form
        {
            var form = _serviceProvider.GetRequiredService<TForm>();
            form.Show();
            current.Close();
        }

        public void ShowDialog<TForm>() where TForm : Form
        {
            var form = _serviceProvider.GetRequiredService<TForm>();
            form.ShowDialog();
        }
    }
}
