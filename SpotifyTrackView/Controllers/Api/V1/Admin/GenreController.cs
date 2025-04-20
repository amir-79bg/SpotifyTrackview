using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;
using SpotifyTrackView.DataTransferObjects;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Controllers.Api.V1.Admin;

[ApiController]
[Route("api/v1/admin/genres")]
public class GenreController : ControllerBase
{
    ApplicationDbContext _context;

    // ReSharper disable once ConvertToPrimaryConstructor
    public GenreController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public async Task<IActionResult> Get()
    {
        List<Genre> genres = await _context.Genres.Include(e => e.SubGenres).ToListAsync();

        return Ok(BuildGenreTreeWithoutParentLoop(genres));
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public async Task<IActionResult> Create([FromBody] CreateGenreDto createGenreDto)
    {
        Genre genre = new Genre
        {
            Name = createGenreDto.Name,
            ParentGenreId = createGenreDto.ParentId
        };

        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();


        return Ok(genre);
    }
    
    
    [HttpDelete("delete/{id:int}")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        
        
        if (genre is null)
            return NotFound(new { message = "Genre not found." });

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
    private List<Genre> BuildGenreTreeWithoutParentLoop(List<Genre> genres, int? parentId = null)
    {
        return genres
            .Where(g => g.ParentGenreId == parentId)
            .Select(g =>
            {
                // Recursively build sub-genres
                var children = BuildGenreTreeWithoutParentLoop(genres, g.Id);

                // Assign them
                g.SubGenres = children;

                // Break the circular reference
                g.ParentGenre = null;

                return g;
            })
            .ToList();
    }
}