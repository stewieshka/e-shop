using IdentityService.Domain;

namespace IdentityService.Persistence.Repositories;

public class UserRepository
{
    private readonly List<User> _users = [];
    
    public async Task<bool> AddAsync(User user)
    {
        await Task.CompletedTask;
        
        _users.Add(user);

        return true;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;

        var user = _users.Find(x => x.Email == email);

        return user;
    }
}