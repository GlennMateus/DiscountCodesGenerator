using DiscountCodesGenerator.Repositories.DiscountCodeRespository;

namespace DiscountCodesGenerator.Services.DiscountCodes.Consume;

public class Handler(IDiscountCodeRepository _repository
    , ILogger<Handler> _logger)
    : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔧 Executing discount code usage: {Code} ...", request.Code);

        try
        {
            return new Result(await _repository.IncrementCodeUsageAsync(request.Code, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Unhandled error has occurred. Please contact an administrator.");
            throw;
        }
    }
}
