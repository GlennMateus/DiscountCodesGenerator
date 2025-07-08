namespace DiscountCodesGenerator.Repositories.DiscountCodeRespository;

public interface IDiscountCodeRepository
{
    Task BulkInsertAsync(IEnumerable<DiscountCode> codes, CancellationToken cancellationToken);
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken);
    Task<bool> IncrementCodeUsageAsync(string code, CancellationToken cancellationToken);
}