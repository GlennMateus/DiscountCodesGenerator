using DiscountCodesGenerator.Repositories.DiscountCodeRespository;

namespace DiscountCodesGenerator.Services.DiscountCodes.GetDiscountCodesService;

public record GetDiscountCodesRequest() : IRequest<GetDiscountCodesResponse>;
public record GetDiscountCodesResponse(IEnumerable<string> Codes);
public class GetDiscountCodesQueryHandler(IDiscountCodeRepository _repository
    , ILogger<GetDiscountCodesQueryHandler> _logger)
    : IRequestHandler<GetDiscountCodesRequest, GetDiscountCodesResponse>
{
    public async Task<GetDiscountCodesResponse> Handle(GetDiscountCodesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔧 Processing GetCodes request");

		try
		{
            var discountCodes = await _repository.GetCodesAsync(cancellationToken);
            var response = discountCodes.Adapt<GetDiscountCodesResponse>();
            return response;
        }
		catch (Exception ex)
		{
            _logger.LogError(ex, "❌ Unhandled error has occurred. Please contact an administrator.");
            throw;
		}
    }
}

