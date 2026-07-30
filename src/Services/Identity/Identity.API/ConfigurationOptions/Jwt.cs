namespace Identity.API.ConfigurationOptions
{
    public class Jwt
    {
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpirationInMinutes { get; init; } = 0;
        public string SecretKey { get; init; } = string.Empty;
        public int RefreshTokenExpirationInDays { get; set; } = 0;
    }
}
