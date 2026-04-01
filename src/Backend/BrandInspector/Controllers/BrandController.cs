using BrandInspector.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BrandInspector.Controllers;

[Authorize]
[ApiController]
[Route("brand")]
public class BrandController(IBrandConfigService brandConfigService) : ControllerBase
{

    [HttpGet("fonts")]
    public async Task<IActionResult> GetFonts(CancellationToken cancellationToken)
    {
        return Ok(await brandConfigService.GetBrandFonts(cancellationToken));
    }

    [HttpGet("colors")]
    public async Task<IActionResult> GetColors(CancellationToken cancellationToken)
    {
        return Ok(await brandConfigService.GetBrandColors( cancellationToken));
    }

    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizes(CancellationToken cancellationToken)
    {
        return Ok(await brandConfigService.GetBrandSizes(cancellationToken));
    }

}
