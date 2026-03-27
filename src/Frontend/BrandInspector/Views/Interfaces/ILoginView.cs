using System;

namespace BrandInspector.Views
{
    public interface ILoginView
    {
        string Username { get; }
        string Password { get; }
        void ShowError(string message);
        void CloseView();

        event EventHandler LoginClicked;

    }
}
