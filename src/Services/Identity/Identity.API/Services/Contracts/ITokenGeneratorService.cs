namespace Identity.API.Services.Contracts
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(Guid userId, string? username, string? email, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
