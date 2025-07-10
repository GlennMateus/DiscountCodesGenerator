using Grpc.Core;
using Grpc.Net.Client;

namespace Web.DiscountCodesGenerator.Services;

public class GrpcDiscountCodesClient(IConfiguration _configuration)
{
    public async Task<GenerateResponse> GenerateCodes(uint count, uint length)
    {
        try
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
            var client = new Generator.GeneratorClient(channel);
            var request = new GenerateRequest
            {
                Count = count,
                Length = length
            };

            var response = await client.GenerateCodesAsync(request);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
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
        var client = new CodeConsumer.CodeConsumerClient(channel);
        var request = new ConsumeCodeRequest
        {
            Code = code
        };

        var response = await client.ConsumeCodeAsync(request);

        return response;
    }
}
