using ProductService.Grpc;
using ProductService.Persistence;
using ProductService.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddGrpc();

    builder.Services.AddScoped<ProductsRepository>();

    builder.Services.AddScoped<IDbConnectionFactory>(_ =>
        new NpgsqlConnectionFactory(
            builder.Configuration[DbConstants.DefaultConnectionStringPath]!));
}

var app = builder.Build();
{
    app.MapGrpcService<ProductsService>();
    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client");
    
    DbInitializer.Initialize(app.Configuration[DbConstants.DefaultConnectionStringPath]!);

    app.Run();
}