using DiscountCodesGenerator;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace Web.DiscountCodesGenerator.Services;

public class GrpcDiscountCodesClient(IConfiguration _configuration) 
    : IGrpcDiscountCodesClient
{
    public async Task<GenerateResponse> GenerateCodesAsync(uint count, uint length)
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

    public async Task<ConsumeCodeResponse> ConsumeCodeAsync(string code)
    {
        using var channel = GrpcChannel.ForAddress(_configuration["GrpcServerUrl"]);
        var client = new ConsumeCodeService.ConsumeCodeServiceClient(channel);
        var request = new ConsumeCodeRequest
        {
            Code = code
        };

        var response = await client.ConsumeCodeAsync(request);

        return response;
    }

    public async Task<GetCodesResponse> GetDiscountCodesAsync() 
    {
        using var channel = GrpcChannel.ForAddress(_configuration["GrpcServerUrl"]);
        var client = new GetCodesService.GetCodesServiceClient(channel);


        var response = await client.GetCodesAsync(new Empty());

        return response;
    }
}
