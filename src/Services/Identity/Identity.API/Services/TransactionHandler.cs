namespace Identity.API.Services
{
    public class TransactionHandler(AppDbContext DbContext) : ITransactionHandler
    {
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
