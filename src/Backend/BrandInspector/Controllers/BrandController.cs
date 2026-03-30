using BrandInspector.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BrandInspector.Controllers;

[ApiController]
[Route("brand")]
public class BrandController(IBrandConfigService brandConfigService) : ControllerBase
{

    [HttpGet("fonts")]
    public async Task<IActionResult> GetFonts()
    {
        return Ok(await brandConfigService.GetBrandFonts());
    }

    [HttpGet("colors")]
    public async Task<IActionResult> GetColors()
    {
        return Ok(await brandConfigService.GetBrandColors());
    }

    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizes()
    {
        return Ok(await brandConfigService.GetBrandSizes());
    }

}
