using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

[ApiController]
[Route("/api/v1/basic-info")]
public class BasicInfoController: BaseApiController
{
    private readonly ApplicationDbContext _context;
    
    public BasicInfoController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _context.Countries.ToListAsync();
        
        return Ok(countries);
    }
    
    [HttpGet]
    [Route("regions")]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _context.Regions.ToListAsync();
        
        return Ok(regions);
    }
}