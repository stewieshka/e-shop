using System.Reflection;
using DbUp;

namespace Infrastructure.Persistence.Database;

public static class DbInitializer
{
    public static void Initialize(string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges
            .To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DbInitializer).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        
        if (!result.Successful)
        {
            throw new InvalidOperationException("Failed migrating database");
        }
    }
}