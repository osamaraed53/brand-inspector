using BrandInspector.Views.Interfaces;
using System;

namespace BrandInspector.Views
{
    public interface ILoginView : IView
    {
        string Username { get; }
        string Password { get; }
        void ShowError(string message);

        event EventHandler LoginClicked;

    }
}
