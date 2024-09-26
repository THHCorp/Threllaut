var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Threllaut_ApiService>("apiservice");

builder.AddProject<Projects.Threllaut_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
