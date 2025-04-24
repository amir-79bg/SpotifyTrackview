using System.Text.Json;
using SpotifyTrackView.Data;
using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedCountries(ApplicationDbContext db)
    {
        if (!db.Countries.Any())
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Seeders/statics", "countries.json");
            var countriesJson = await File.ReadAllTextAsync(path);
            

            using var doc = JsonDocument.Parse(countriesJson);
            var root = doc.RootElement;

            var countryEntities = new List<Country>();

            foreach (var item in root.EnumerateArray())
            {
                string iso = item.GetProperty("iso").GetString()!;
                string name = item.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? iso : iso;

                countryEntities.Add(new Country
                {
                    Name = name,
                    Iso2 = iso, // required by your DB
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }


            db.Countries.AddRange(countryEntities);

            await db.SaveChangesAsync();
        }
    }

    public static async Task SeedRegions(ApplicationDbContext db)
    {
        if (!db.Regions.Any())
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Seeders/statics", "region.json");
            var regionsJson = await File.ReadAllTextAsync(path);

            using var doc = JsonDocument.Parse(regionsJson);
            var root = doc.RootElement;

            var regionsEntity = new List<Region>();
            
            foreach (var item in root.EnumerateArray())
            {
                string iso = item.GetProperty("iso").GetString()!;

                regionsEntity.Add(new Region
                {
                    Iso2 = iso, // required by your DB
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            db.Regions.AddRange(regionsEntity);

            await db.SaveChangesAsync();
        }
    }
    
    public static async Task SeedGenres(ApplicationDbContext db)
    {
        if (!db.Genres.Any())
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Seeders/statics", "genres.json");
            var genresJson = await File.ReadAllTextAsync(path);

            using var doc = JsonDocument.Parse(genresJson);
            var root = doc.RootElement;

            var genresToAdd = new List<Genre>();

            foreach (var item in root.EnumerateArray())
            {
                var parent = new Genre
                {
                    Name = item.GetProperty("name").GetString()!,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Check for subGenres
                if (item.TryGetProperty("subGenres", out var subGenres))
                {
                    foreach (var subItem in subGenres.EnumerateArray())
                    {
                        parent.SubGenres.Add(new Genre
                        {
                            Name = subItem.GetProperty("name").GetString()!,
                            ParentGenre = parent,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }
                }

                genresToAdd.Add(parent);
            }

            db.Genres.AddRange(genresToAdd);
            await db.SaveChangesAsync();
        }
    }
}