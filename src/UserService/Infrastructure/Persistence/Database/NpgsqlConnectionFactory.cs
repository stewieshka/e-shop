using System.Data;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Npgsql;

namespace Infrastructure.Persistence.Database;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        return connection;
    }
}