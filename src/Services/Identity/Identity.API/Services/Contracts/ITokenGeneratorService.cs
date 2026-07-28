namespace Identity.API.Services.Contracts
{
    internal interface ITokenGeneratorService
    {
        string GenerateToken(string userId, string? username, string? email, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
