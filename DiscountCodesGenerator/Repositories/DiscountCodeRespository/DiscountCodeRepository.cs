
using Microsoft.Data.SqlClient;
using System.Data;

namespace DiscountCodesGenerator.Repositories.DiscountCodeRespository;

public class DiscountCodeRepository(IConfiguration configuration
    , AppDbContext _db
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
        return await _db
            .DiscountCodes
            .AsNoTracking()
            .AnyAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<DiscountCode> GetCodeAsync(string code, CancellationToken cancellationToken)
    {
        _logger.LogInformation("💾 Retrieving code from Db...");
        return await _db.DiscountCodes
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    => _db.SaveChangesAsync(cancellationToken);
}
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace DiscountCodesGenerator.Repositories.DiscountCodeRespository;

//public class DiscountCodeRepository(
//    IConfiguration configuration,
//    IDbContextFactory<AppDbContext> dbContextFactory,
//    ILogger<DiscountCodeRepository> _logger)
//    : IDiscountCodeRepository
//{
//    private readonly string _connectionString = configuration.GetConnectionString("SQL")!;
//    private readonly IDbContextFactory<AppDbContext> _dbContextFactory = dbContextFactory;


//    public async Task BulkInsertAsync(IEnumerable<DiscountCode> codes, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("💾 Saving discount codes...");
//        try
//        {
//            using var connection = new SqlConnection(_connectionString);
//            await connection.OpenAsync(cancellationToken);

//            using var bulkCopy = new SqlBulkCopy(connection)
//            {
//                DestinationTableName = "DiscountCodes",
//                BatchSize = 500
//            };

//            var table = new DataTable();
//            table.Columns.Add("Id", typeof(Guid));
//            table.Columns.Add("Code", typeof(string));

//            foreach (var code in codes)
//            {
//                table.Rows.Add(code.Id, code.Code);
//            }

//            await bulkCopy.WriteToServerAsync(table, cancellationToken);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "❌ Error while bulk-inserting generated codes into DB.");
//            throw;
//        }
//    }

//    public async Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("⚙️ Verifying generated code...");

//        // Create a new DbContext instance for this operation
//        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

//        return await db
//            .DiscountCodes
//            .AsNoTracking()
//            .AnyAsync(x => x.Code == code, cancellationToken);
//    }

//    public async Task<DiscountCode?> GetCodeAsync(string code, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("💾 Retrieving code from Db...");

//        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

//        return await db.DiscountCodes
//            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
//    }
//}
