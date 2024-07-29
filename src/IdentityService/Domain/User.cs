namespace IdentityService.Domain;

public class User(Guid id, string email, string username, string passwordHash)
{
    public Guid Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string Username { get; set; } = username;
    public string PasswordHash { get; set; } = passwordHash;
}