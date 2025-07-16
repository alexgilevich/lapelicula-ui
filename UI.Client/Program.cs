using LaPelicula.UI.Client;
using LaPelicula.UI.Client.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UI.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddLogging()
    .AddScoped<IUserPreferencesEncoder, UserPreferencesEncoder>()
    .AddScoped<IRecommendationsHttpService, RecommendationsHttpService>()
    .AddScoped<HttpClient>(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
