using Google.Protobuf.WellKnownTypes;

namespace DiscountCodesGenerator.Services.DiscountCodes.Get;

public class Endpoint(IMediator sender) 
    : GetCodesService.GetCodesServiceBase
{
    public override async Task<GetCodesResponse> GetCodes(Empty emptyRequest
        , ServerCallContext context)
    {
        var query = new Query();
        var result = await sender.Send(query);
        var response = new GetCodesResponse();
        response.Codes.AddRange(result.Codes);

        return response;
    }
}
