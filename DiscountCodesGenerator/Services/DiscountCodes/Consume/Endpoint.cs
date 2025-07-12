namespace DiscountCodesGenerator.Services.DiscountCodes.Consume;

public class Endpoint(IMediator sender) : ConsumeCodeService.ConsumeCodeServiceBase
{
    public override async Task<ConsumeCodeResponse> ConsumeCode(ConsumeCodeRequest request,
        ServerCallContext context)
    {
        var command = request.Adapt<Command>();
        var result = await sender.Send(command);
        var response = result.Adapt<ConsumeCodeResponse>();
        return response;
    }
}
