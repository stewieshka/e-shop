namespace Domain.Sessions;

public class Session
{
    public Guid RefreshToken { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public required string Ip { get; set; }
    public required string UserAgent { get; set; }
}