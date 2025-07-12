using DiscountCodesGenerator.Repositories.DiscountCodeRespository;

namespace DiscountCodesGenerator.Services.DiscountCodes.Get;

public class Handler(IDiscountCodeRepository _repository
    , ILogger<Handler> _logger)
    : IRequestHandler<Query, Result>
{
    public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔧 Processing GetCodes request");

		try
		{
            var discountCodes = await _repository.GetCodesAsync(cancellationToken);
            var response = discountCodes.Adapt<Result>();
            return response;
        }
		catch (Exception ex)
		{
            _logger.LogError(ex, "❌ Unhandled error has occurred. Please contact an administrator.");
            throw;
		}
    }
}

