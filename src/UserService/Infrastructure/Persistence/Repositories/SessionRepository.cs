using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Persistence.Repositories;
using Dapper;
using Domain.Sessions;
using Domain.Users;

namespace Infrastructure.Persistence.Repositories;

public class SessionRepository(
    IDbConnectionFactory dbConnectionFactory) : ISessionRepository
{
    public async Task<int> CreateAsync(Session session)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             INSERT INTO sessions (refresh_token, user_id, expires_at, ip, user_agent)
                             VALUES (@RefreshToken, @UserId, @ExpiresAt, @Ip, @UserAgent);
                             """;

        var result = await connection.ExecuteAsync(query, session);

        return result;
    }

    public async Task<Session?> GetById(Guid refreshToken)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = """
                             SELECT refresh_token AS RefreshToken, user_id AS UserId, expires_at AS ExpiresAt, ip AS Ip, user_agent AS UserAgent
                             FROM sessions
                             WHERE refresh_token = @RefreshToken
                             """;

        var session = await connection.QueryFirstOrDefaultAsync<Session>(query, new { RefreshToken = refreshToken });

        return session;
    }

    public async Task<int> DeleteAsync(Guid refreshToken)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();

        const string query = "DELETE FROM sessions WHERE refresh_token = @RefreshToken";

        var affectedRows = await connection.ExecuteAsync(query, new { RefreshToken = refreshToken });

        return affectedRows;
    }
}