using LaPelicula.UI.Client.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddLogging()
    .AddScoped<IUserPreferencesEncoder, UserPreferencesEncoder>()
    .AddScoped<IRecommendationsHttpService, RecommendationsHttpService>()
    .AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
