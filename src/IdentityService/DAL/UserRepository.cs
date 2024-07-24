using IdentityService.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DAL;

public class UserRepository(AppDbContext context)
{
    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }
}