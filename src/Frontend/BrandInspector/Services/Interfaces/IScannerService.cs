using BrandInspector.Models;
using System.Collections.Generic;
using System.Threading;

namespace BrandInspector.Services.Interfaces
{
    public interface IScannerService
    {
        List<TextRunInfo> ScanPresentation(string filePath, CancellationToken token);

    }
}
