using MyAPI.Enums;

namespace MyAPI.AuthorizationAndAuthentication
{
    public class TokenConfiguration
    {
        public string? Secret { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int ExpirationTimeInHours { get; set; }
        public string? Username { get; set; }
        public UserRole Role { get; set; }
    }
}
