using BrandInspector.Models;
using System;
using System.Collections.Generic;

namespace BrandInspector.Views
{
    public interface IMainView 
    {
        string SelectedFilePath { get; set; }
        void ShowMessage(string message);
    }
}
