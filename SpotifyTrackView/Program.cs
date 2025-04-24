using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpotifyTrackView.Data;
using SpotifyTrackView.Entity;
using SpotifyTrackView.Interfaces;
using SpotifyTrackView.Options;
using SpotifyTrackView.Seeders;
using SpotifyTrackView.Services;
using SpotifyTrackView.Validation.Rules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection("FileUpload")
);
// Load JWT config
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
var adminSettings = jwtConfig.GetSection("Admin");
var artistSettings = jwtConfig.GetSection("Artist");
var influencerSettings = jwtConfig.GetSection("Influencer");
var listenerSettings = jwtConfig.GetSection("Listener");

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.HttpContext.Request.Path
        };

        foreach (var error in errors)
        {
            problemDetails.Errors.Add(error.Key, error.Value);
        }

        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/json" }
        };
    };
});

builder.Services.AddScoped<IPasswordHasher<Admin>, PasswordHasher<Admin>>();
builder.Services.AddScoped<IPasswordHasher<Influencer>, PasswordHasher<Influencer>>();
builder.Services.AddScoped<IPasswordHasher<Artist>, PasswordHasher<Artist>>();
builder.Services.AddScoped<IPasswordHasher<Listener>, PasswordHasher<Listener>>();
// AUth Service
builder.Services.AddScoped<IAuthService<Admin>, AuthService<Admin>>();
builder.Services.AddScoped<IAuthService<Influencer>, AuthService<Influencer>>();
builder.Services.AddScoped<IAuthService<Artist>, AuthService<Artist>>();
builder.Services.AddScoped<IAuthService<Listener>, AuthService<Listener>>();


builder.Services.AddAuthentication()
    .AddJwtBearer("AdminScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = adminSettings["Issuer"],
            ValidAudience = adminSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(adminSettings["SecretKey"] ??
                                                                               throw new InvalidOperationException(
                                                                                   "Admin settings not-found.")))
        };
    }).AddJwtBearer("ArtistScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = artistSettings["Issuer"],
            ValidAudience = artistSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(artistSettings["SecretKey"] ??
                                                                               throw new InvalidOperationException(
                                                                                   "Artist settings not-found.")))
        };
    }).AddJwtBearer("InfluencerScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = influencerSettings["Issuer"],
            ValidAudience = influencerSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(influencerSettings["SecretKey"] ??
                                                                               throw new InvalidOperationException(
                                                                                   "Influencer settings not-found.")))
        };
    }).AddJwtBearer("ListenerScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = listenerSettings["Issuer"],
            ValidAudience = listenerSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(listenerSettings["SecretKey"] ??
                                                                               throw new InvalidOperationException(
                                                                                   "Listener settings not-found.")))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container. mapper pattern
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connection);
});

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileValidator>();



builder.Services.AddSwaggerGen(c =>
{
    // Define the security schemes for all user types
    var schemes = new[] { "AdminScheme", "ArtistScheme", "InfluencerScheme", "ListenerScheme" };

    foreach (var scheme in schemes)
    {
        c.AddSecurityDefinition(scheme, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = $"JWT Authorization header using the Bearer scheme for {scheme}. Enter your token only."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = scheme
                    }
                },
                Array.Empty<string>()
            }
        });
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await DatabaseSeeder.SeedCountries(db);
    await DatabaseSeeder.SeedRegions(db);
    await DatabaseSeeder.SeedGenres(db);
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();