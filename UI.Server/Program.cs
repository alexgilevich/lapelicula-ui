using LaPelicula.UI.Server.Components;
using CSnakes.Runtime;
using CSnakes.Runtime.Locators;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.Extensions.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var home = Path.Join(Environment.CurrentDirectory, "ml");
builder.Services
    .WithPython()
    .WithHome(home)
    .FromRedistributable(RedistributablePythonVersion.Python3_11)
    .WithVirtualEnvironment(Path.Join(home, ".venv"))
    .WithPipInstaller(Path.Join(home, "requirements.txt"));

builder.Services
    .AddLogging()
    .AddMemoryCache()
    .AddSingleton<IUserPreferencesEncoder, UserPreferencesEncoder>()
    .AddSingleton<ITensorFlowModelService, TensorFlowModelService>()
    .AddSingleton<IMovieRepository, InMemoryMovieRepository>()
    .AddSingleton<IRecommendationService, RecommendationService>()
    .AddHttpContextAccessor()
    .AddControllers();

// Model training hosted service – immediately starts training when the application starts
builder.Services.AddHostedService<ModelTrainingHostedService>();
builder.Services.Configure<HostOptions>(options =>
{
    //Service Behavior in case of exceptions - defaults to StopHost
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LaPelicula.UI.Client._Imports).Assembly);
app.Run();