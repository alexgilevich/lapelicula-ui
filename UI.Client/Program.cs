using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddLogging()
    .AddScoped<IUserPreferencesEncoder, UserPreferencesEncoder>();

await builder.Build().RunAsync();
