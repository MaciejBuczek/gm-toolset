namespace Identity.API.Services.Contracts
{
    internal interface ITokenGeneratorService
    {
        string GenerateToken(Guid userId, string? username, string? email, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
