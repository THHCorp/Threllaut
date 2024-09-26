var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver");
var apiDatabase = sqlServer.AddDatabase("apidatabase");

var apiService = builder.AddProject<Projects.Threllaut_ApiService>("apiservice")
    .WithReference(apiDatabase);

builder.AddProject<Projects.Threllaut_Web>("webapp")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

await builder.Build().RunAsync();
