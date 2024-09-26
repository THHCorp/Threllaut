using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Threllaut.Shared.Services;
using Threllaut.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    client.BaseAddress = new("https://apiservice"));

await builder.Build().RunAsync();
