using Domain.Users;

namespace Application.Common.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}