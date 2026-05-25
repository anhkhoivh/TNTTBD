using MediatR;
using TNTTBD.Services.Api.Common.Localization;
using TNTTBD.Services.Application.Behaviors;
using TNTTBD.Services.Application.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseCors();

app.UseRequestLocalization();

app.UseAuthorization();

app.MapControllers();

app.Run();
