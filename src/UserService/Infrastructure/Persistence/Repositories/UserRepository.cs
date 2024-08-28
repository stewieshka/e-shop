using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Persistence.Repositories;
using Dapper;
using Domain.Users;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             SELECT id AS Id, username AS Username, email AS Email, password_salt AS PasswordSalt, password_hash AS PasswordHash
                             FROM users
                             WHERE email = @Email
                             """;

        var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

        return user;
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             INSERT INTO users (id, username, email, password_salt, password_hash)
                             VALUES (@Id, @Username, @Email, @PasswordSalt, @PasswordHash);
                             """;

        var result = await connection.ExecuteAsync(query, user);

        return result;
    }

    public async Task<User?> GetById(Guid id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             SELECT id AS Id, username AS Username, email AS Email, password_salt AS PasswordSalt, password_hash AS PasswordHash
                             FROM users
                             WHERE id = @Id
                             """;

        var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });

        return user;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = "DELETE FROM users WHERE id = @Id";

        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });

        return affectedRows;
    }
}