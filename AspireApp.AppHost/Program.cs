using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379);

var statestore = builder.AddDaprStateStore(
    "statestore",
    new DaprComponentOptions
    {
        LocalPath = Path.Combine("..", "components", "statestore.yaml")
    });

var api = builder
    .AddProject<Projects.AspireApp_API>("api")
    .WithDaprSidecar()
    .WithReference(statestore);

builder.Build().Run();
