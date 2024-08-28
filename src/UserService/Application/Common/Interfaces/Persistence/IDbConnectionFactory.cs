using System.Data;

namespace Application.Common.Interfaces.Persistence;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}