using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SpotifyTrackView.Data;

namespace SpotifyTrackView.Validation.Attributes;

public class ExistsAttribute(string table, string column) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        var db = validationContext.GetService<ApplicationDbContext>();
        if (db == null)
        {
            throw new Exception("Could not resolve ApplicationDbContext.");
        }

        var sql = $"SELECT COUNT(1) FROM [{table}] WHERE [{column}] = @p0";
        var exists = db.Database.ExecuteSqlRaw(sql, value) > 0;

        return exists
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? $"{column} does not exist in {table}.");
    }
}