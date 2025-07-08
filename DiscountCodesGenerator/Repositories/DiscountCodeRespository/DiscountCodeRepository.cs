
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DiscountCodesGenerator.Repositories.DiscountCodeRespository;

public class DiscountCodeRepository(IConfiguration configuration
    , IDbContextFactory<AppDbContext> _dbContextFactory
    , ILogger<DiscountCodeRepository> _logger)
    : IDiscountCodeRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("SQL")!;

    public async Task BulkInsertAsync(IEnumerable<DiscountCode> codes
        , CancellationToken cancellationToken)
    {
        _logger.LogInformation("💾 Saving discount codes...");
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "DiscountCodes",
                BatchSize = 500
            };

            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("Code", typeof(string));

            foreach (var code in codes)
            {
                table.Rows.Add(code.Id, code.Code);
            }

            await bulkCopy.WriteToServerAsync(table, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error while bulk-inserting generated codes into DB.");
            throw;
        }
    }

    public async Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken)
    {
        _logger.LogInformation("⚙️ Verifying generated code...");
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext
            .DiscountCodes
            .AsNoTracking()
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<bool> IncrementCodeUsageAsync(string code, CancellationToken cancellationToken)
    {
        _logger.LogInformation("💾 Incrementing code usage if it exists in Db...");
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var rowsAffected = await dbContext.DiscountCodes
       .Where(c => c.Code == code)
       .ExecuteUpdateAsync(
           updates => updates.SetProperty(c => c.TimesUsed, c => c.TimesUsed + 1),
           cancellationToken);

        return rowsAffected > 0;
    }
}