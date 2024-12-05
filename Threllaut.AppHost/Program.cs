var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddSqlServer("sqlserver")
    .AddDatabase("database");

builder.AddProject<Projects.Threllaut_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(database);

builder.AddProject<Projects.Threllaut_Web>("webapp")
    .WithExternalHttpEndpoints()
    .WithReference(database);

await builder.Build().RunAsync();
