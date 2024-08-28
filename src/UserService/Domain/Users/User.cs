namespace Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public required string PasswordSalt { get; set; }
    public required string PasswordHash { get; set; }
}