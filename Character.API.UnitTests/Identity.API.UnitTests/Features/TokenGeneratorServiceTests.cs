namespace Identity.API.UnitTests.Features
{
    public class TokenGeneratorServiceTests
    {
        private readonly IOptions<Jwt> _options = Options.Create(new Jwt()
        {
            SecretKey = "SecretKeyItHasToHasAValidLengthSoIMakeItLong",
            ExpirationInMinutes = 60,
            Issuer = "Issuer",
            Audience = "Audience",
            RefreshTokenExpirationInDays = 7
        });

        private Guid _userId = Guid.NewGuid();
        private string _username = "testuser";
        private string _email = "test@email.com";
        private string[] _roles = ["role1", "role2"];

        [Fact]
        public void GenerateRefreshToken_ShouldReturnBase64String()
        {
            // Arrange
            var tokenGeneratorService = new TokenGeneratorService(_options);

            //Act
            var refreshToken = tokenGeneratorService.GenerateRefreshToken();
            var bytes = Convert.FromBase64String(refreshToken);

            //Assert
            Assert.Equal(32, bytes.Length);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnUniqueTokens()
        {
            // Arrange
            var tokenGeneratorService = new TokenGeneratorService(_options);

            // Act
            var refreshToken1 = tokenGeneratorService.GenerateRefreshToken();
            var refreshToken2 = tokenGeneratorService.GenerateRefreshToken();

            // Assert
            Assert.NotEqual(refreshToken1, refreshToken2);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var tokenGeneratorService = new TokenGeneratorService(_options);

            //Act
            var token = tokenGeneratorService.GenerateToken(_userId, _username, _email, _roles);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            //Assert
            Assert.NotNull(jwtToken);
        }

        [Fact]
        public void GenerateToken_ShouldContainCorrectClaims()
        {
            // Arrange
            var tokenGeneratorService = new TokenGeneratorService(_options);

            //Act
            var token = tokenGeneratorService.GenerateToken(_userId, _username, _email, _roles);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            //Assert
            Assert.Equal(_userId.ToString(), jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            Assert.Equal(_username, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
            Assert.Equal(_email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
            foreach (var role in _roles)
            {
                Assert.Contains(jwtToken.Claims, c => c.Type == "role" && c.Value == role);
            }
        }

        [Fact]
        public void GenerateToken_ShouldHaveCorrectData() 
        {             
            // Arrange
            var tokenGeneratorService = new TokenGeneratorService(_options);

            //Act
            var token = tokenGeneratorService.GenerateToken(_userId, _username, _email, _roles);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            //Assert
            Assert.Equal(_options.Value.Issuer, jwtToken.Issuer);
            Assert.Equal(_options.Value.Audience, jwtToken.Audiences.First());
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        }
    }
}
