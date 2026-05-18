using Amazon.DynamoDBv2;
using LaPelicula.UI.Server.Components;
using CSnakes.Runtime;
using CSnakes.Runtime.Locators;
using LaPelicula.UI.Server.Common;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Server.Services.HostedServices;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Internal;
using UI.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Python services to the container.
var home = Path.Join(Environment.CurrentDirectory, "Python");
var pythonBuilder =builder.Services
    .WithPython()
    .WithHome(home)
    .WithVirtualEnvironment(Path.Join(home, ".venv-net"));

if (builder.Environment.IsDevelopment())
{
    pythonBuilder
        .FromRedistributable(RedistributablePythonVersion.Python3_13)
        .WithPipInstaller(Path.Join(home, "requirements.txt"));
}
else
{
    var dottedVersion = "3.13.2";
    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.DoNotVerify);
    var installedPath = Path.Join(appDataPath, "CSnakes", $"python{dottedVersion}", "python", "install");
    pythonBuilder
        .FromFolder(installedPath, dottedVersion);
}


// Add services to the container.
builder.Services
    .AddLogging()
    .AddMemoryCache()
    .AddSingleton<IUserPreferencesEncoder, UserPreferencesEncoder>()
    .AddSingleton<ITensorFlowModelService, TensorFlowModelService>()
    .AddSingleton<IMovieRepository, DynamoDbMovieRepository>()
    .AddSingleton<IMovieService, MovieService>()
    .AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>()
    .AddSingleton<IRecommendationService, RecommendationService>()
    .AddHttpContextAccessor()
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
    


// Model training hosted service – immediately starts training when the application starts
builder.Services.AddHostedService<ModelTrainingHostedService>();
builder.Services.Configure<HostOptions>(options =>
{
    //Service Behavior in case of exceptions - defaults to StopHost
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

// Configure X-Forwarded header for the HSTS and HttpsRedirect middleware to work correctly
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.Configure<RecommendationsConfig>(builder.Configuration.GetSection("Recommendations"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Enable X-Forwarded headers in production
    app.UseForwardedHeaders();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Pipeline
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();