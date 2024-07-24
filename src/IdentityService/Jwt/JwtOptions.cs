namespace IdentityService.Jwt;

public class JwtOptions(string secretKey, int expiryHours)
{
    public string SecretKey { get; set; } = secretKey;
    public int ExpiryHours { get; set; } = expiryHours;
}