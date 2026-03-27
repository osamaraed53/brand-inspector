using BrandInspector.Models;
using System.Collections.Generic;
using System;
using BrandInspector.Views.Interfaces;

namespace BrandInspector.Views
{
    public interface IMainView 
    {
        string SelectedFilePath { get; set; }

        void DisplayResults(IList<TextRunInfo> results);

        event EventHandler BrowseClicked;
        event EventHandler ScanFontsClicked;
        event EventHandler ScanColorsClicked;
        event EventHandler ScanSizesClicked;

    }
}
