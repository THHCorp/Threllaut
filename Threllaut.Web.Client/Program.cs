using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Threllaut.Shared.Services;
using Threllaut.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
