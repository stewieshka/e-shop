using IdentityService.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DAL;

public class AppDbContext(
    DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}