using DiscountCodesGenerator;
using Grpc.Net.Client;

namespace Web.DiscountCodesGenerator.Services;

public class GrpcDiscountCodesClient(IConfiguration _configuration)
{
    public async Task<GenerateResponse> GenerateCodes(uint count, uint length)
    {
        using var channel = GrpcChannel.ForAddress(_configuration["GrpcServerUrl"]);
        var client = new GenerateCodesService.GenerateCodesServiceClient(channel);
        var request = new GenerateRequest
        {
            Count = count,
            Length = length
        };

        var response = await client.GenerateCodesAsync(request);

        return response;
    }

    public async Task<ConsumeCodeResponse> ConsumeCode(string code)
    {
        using var channel = GrpcChannel.ForAddress(
            _configuration["GrpcServerUrl"]
            , new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            });
        var client = new ConsumeCodeService.ConsumeCodeServiceClient(channel);
        var request = new ConsumeCodeRequest
        {
            Code = code
        };

        var response = await client.ConsumeCodeAsync(request);

        return response;
    }
}
