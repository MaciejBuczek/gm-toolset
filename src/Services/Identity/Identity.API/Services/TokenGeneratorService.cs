namespace Identity.API.Services
{
    public class TokenGeneratorService(IOptions<Jwt> JwtOptions) : ITokenGeneratorService
    {
        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(Guid userId, string? username, string? email, IEnumerable<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, username?? string.Empty),
                new(JwtRegisteredClaimNames.Email, email?? string.Empty),
            };
            claims.AddRange(roleClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JwtOptions.Value.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = JwtOptions.Value.Issuer,
                Audience = JwtOptions.Value.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
