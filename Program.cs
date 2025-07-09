using LaPelicula.UI.Components;
using CSnakes.Runtime;
using CSnakes.Runtime.Locators;
using LaPelicula.UI.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var home = Path.Join(Environment.CurrentDirectory, "ml");
builder.Services
    .WithPython()
    .WithHome(home)
    .FromRedistributable(RedistributablePythonVersion.Python3_11)
    .WithVirtualEnvironment(Path.Join(home, ".venv"))
    .WithPipInstaller(Path.Join(home, "requirements.txt"))
    ;

builder.Services
    .AddLogging()
    .AddScoped<IUserPreferencesService, UserPreferencesService>()
    .AddSingleton<ITensorFlowModelService, TensorFlowModelService>()
    .AddHttpContextAccessor();

// Model training hosted service – immediately starts training when the application starts
//builder.Services.AddHostedService<ModelTrainingHostedService>();
builder.Services.Configure<HostOptions>(options =>
{
    //Service Behavior in case of exceptions - defaults to StopHost
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();