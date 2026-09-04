namespace Identity.API.Services.Contracts
{
    public interface ITransactionHandler
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
