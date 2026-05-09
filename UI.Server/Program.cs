using Amazon.DynamoDBv2;
using LaPelicula.UI.Server.Components;
using CSnakes.Runtime;
using CSnakes.Runtime.Locators;
using LaPelicula.UI.Client.Services;
using LaPelicula.UI.Server.Common;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Server.Services.HostedServices;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Internal;
using UI.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add Python services to the container.
var home = Path.Join(Environment.CurrentDirectory, "Python");
builder.Services
    .WithPython()
    .WithHome(home)
    .FromRedistributable(RedistributablePythonVersion.Python3_13)
    .WithVirtualEnvironment(Path.Join(home, ".venv-net"))
    .WithPipInstaller(Path.Join(home, "requirements.txt"));

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
    .AddSingleton<IRecommendationsHttpService, PrerenderedRecommendationsHttpService>()
    .AddHttpContextAccessor()
    .AddControllers();
    


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
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Enable X-Forwarded headers in production
    app.UseForwardedHeaders();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Pipeline
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LaPelicula.UI.Client._Imports).Assembly);
app.Run();