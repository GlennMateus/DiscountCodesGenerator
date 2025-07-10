using DiscountCodesGenerator;

namespace Web.DiscountCodesGenerator.Services;

public interface IGrpcDiscountCodesClient
{
    Task<GenerateResponse> GenerateCodesAsync(uint count, uint length);
    Task<ConsumeCodeResponse> ConsumeCodeAsync(string code);
    Task<GetCodesResponse> GetDiscountCodesAsync();
}
