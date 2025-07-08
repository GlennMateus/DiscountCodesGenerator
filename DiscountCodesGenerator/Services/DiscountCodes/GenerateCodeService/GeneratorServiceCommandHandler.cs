using DiscountCodesGenerator.Repositories.DiscountCodeRespository;
using System.Collections.Concurrent;

namespace DiscountCodesGenerator.Services.DiscountCodes.GenerateCodeService;

public record GenerateCodesCommand(ushort Count, byte Length) : IRequest<GenerateCodesResult>;
public record GenerateCodesResult(IEnumerable<string> Codes);

internal class GeneratorServiceCommandHandler(IDiscountCodeRepository _repository
    , ILogger<GeneratorServiceCommandHandler> _logger)
    : IRequestHandler<GenerateCodesCommand, GenerateCodesResult>
{
    private readonly SemaphoreSlim _dbSemaphore = new(10); // Limit concurrent DB checks

    public async Task<GenerateCodesResult> Handle(GenerateCodesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔧 Generating {Count} discount codes...", request.Count);

        try
        {
            var parallelismOptions = new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = cancellationToken };
            var generatedCodes = new ConcurrentBag<DiscountCode>();

            await Parallel.ForEachAsync(Enumerable.Range(0, request.Count)
                , parallelismOptions
                , async (i, ct) =>
            {
                generatedCodes.Add(new DiscountCode
                {
                    Id = Guid.NewGuid(),
                    Code = await GenerateDiscountCode(request.Length, cancellationToken)
                });
            });

            await _repository.BulkInsertAsync(generatedCodes, cancellationToken);

            return new GenerateCodesResult(generatedCodes.Select(c => c.Code));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Unhandled error has occurred. Please contact an administrator.");
            throw;
        }
    }

    private async Task<string> GenerateDiscountCode(byte length, CancellationToken cancellationToken)
    {
        do
        {
            await _dbSemaphore.WaitAsync(cancellationToken); 
            var code = await Nanoid
                    .GenerateAsync(alphabet: Nanoid.Alphabets.LettersAndDigits, size: length);

            try
            {
                if (!await _repository.CodeExistsAsync(code, cancellationToken))
                    return code;
            }
            finally
            {
                _dbSemaphore.Release();
            }
        } while (true);
    }
}