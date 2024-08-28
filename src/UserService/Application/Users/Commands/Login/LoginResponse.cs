namespace Application.Users.Commands.Login;

public record LoginResponse(
    string AccessToken, Guid RefreshToken, DateTime ExpiresAt);