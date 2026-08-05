using MediatR;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TNTTBD.Services.Api.Common.Localization;
using TNTTBD.Services.Api.Middleware;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Behaviors;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Common;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Infrastructure.Auth;
using TNTTBD.Services.Infrastructure.Persistence;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;
using TNTTBD.Services.Infrastructure.Storage;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS — read allowed origins from configuration
var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSingleton<ILocalizationService, JsonLocalizationService>();

builder.Services.AddScoped<SeederAndMigrationHealthCheck>();
builder.Services.AddHealthChecks()
    .AddCheck<SeederAndMigrationHealthCheck>("SeederAndMigrations", tags: new[] { "ready" });

// Database
TryConfigureDatabase();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

// Repositories
builder.Services.AddScoped<ICatechismYearRepository, CatechismYearRepository>();
builder.Services.AddScoped<ICatechismClassRepository, CatechismClassRepository>();
builder.Services.AddScoped<ICatechismSessionRepository, CatechismSessionRepository>();
builder.Services.AddScoped<IEucharisticYouthRepository, EucharisticYouthRepository>();
builder.Services.AddScoped<ICatechistRepository, CatechistRepository>();
builder.Services.AddScoped<ICatechismClassEucharisticYouthRepository, CatechismClassEucharisticYouthRepository>();
builder.Services.AddScoped<ICatechismClassCatechistRepository, CatechismClassCatechistRepository>();
builder.Services.AddScoped<IEucharisticYouthRunningNumberRepository, EucharisticYouthRunningNumberRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IEucharisticYouthContactRepository, EucharisticYouthContactRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();

// File Storage
var fileStorageBasePath = builder.Configuration["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
builder.Services.AddSingleton<IFileStorage>(new LocalFileStorage(fileStorageBasePath, string.Empty));

// Auth & User services
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("OAuth");
builder.Services.AddScoped<IAuthorizationContext, AuthorizationContext>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOAuthService, OAuthService>();
builder.Services.AddScoped<IExternalLoginRepository, ExternalLoginRepository>();
builder.Services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

// JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "TNTTBD-Super-Secret-Key-Change-In-Production-2026!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "tnttbd",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "tnttbd-ui",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();

// Query Services
builder.Services.AddScoped<ICatechismYearQueryService, CatechismYearQueryService>();
builder.Services.AddScoped<ICatechismClassQueryService, CatechismClassQueryService>();
builder.Services.AddScoped<ICatechismSessionQueryService, CatechismSessionQueryService>();
builder.Services.AddScoped<IEucharisticYouthQueryService, EucharisticYouthQueryService>();
builder.Services.AddScoped<ICatechistQueryService, CatechistQueryService>();
builder.Services.AddScoped<ICatechismClassEucharisticYouthQueryService, CatechismClassEucharisticYouthQueryService>();
builder.Services.AddScoped<ICatechismClassCatechistQueryService, CatechismClassCatechistQueryService>();
builder.Services.AddScoped<IEucharisticYouthContactQueryService, EucharisticYouthContactQueryService>();
builder.Services.AddScoped<IExamQueryService, ExamQueryService>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "vi" };

    options.SetDefaultCulture("vi")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
await TryMigrateDatabase();

app.Run();

void TryConfigureDatabase()
{
    builder.Services.AddCatechismDbContext(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        commandTimeout: TimeSpan.FromSeconds(60),
        options => options.EnableSensitiveDataLogging(!builder.Environment.IsProduction())
    );
}


async Task TryMigrateDatabase()
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Migrating database...");


        scope.ServiceProvider.GetService<CatechismDbContext>()?.Database.Migrate();


        logger.LogInformation("Successfully migrated database.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating database.");
        throw;
    }

    var context = scope.ServiceProvider.GetRequiredService<CatechismDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UserEntity>>();
    if (!context.Users.Any(u => u.Email == "admin@tnttbd.org"))
    {
        var admin = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = "admin@tnttbd.org",
            UserName = "Admin",
            Role = TNTTBD.Services.Domain.Enums.UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };
        admin.PasswordHash = hasher.HashPassword(admin, "changeme");
        context.Users.Add(admin);
        context.SaveChanges();
    }
}
