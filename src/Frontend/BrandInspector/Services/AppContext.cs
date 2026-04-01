using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

public class AppContext : ApplicationContext
{
    private readonly IServiceProvider _serviceProvider;

    public AppContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Show(Form form)
    {
        form.FormClosed += FormClosed;
        form.Show();
    }


    private void FormClosed(object sender, FormClosedEventArgs e)
    {  
        ExitThread();
    }

    public void ShowForm<To>(Form fromForm) where To : Form
    {
        var mainForm = _serviceProvider.GetRequiredService<To>();

        mainForm.FormClosed += FormClosed;
        mainForm.Show();


        fromForm.FormClosed -= FormClosed;
        fromForm.Close();
    }


}