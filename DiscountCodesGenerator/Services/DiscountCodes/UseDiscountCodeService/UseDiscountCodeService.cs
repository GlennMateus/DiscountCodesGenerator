namespace DiscountCodesGenerator.Services.DiscountCodes.UseDiscountCodeService;

public class UseDiscountCodeService(IMediator sender) : ConsumeCodeService.ConsumeCodeServiceBase
{
    public override async Task<ConsumeCodeResponse> ConsumeCode(ConsumeCodeRequest request,
        ServerCallContext context)
    {
        var command = request.Adapt<UseCodeCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<ConsumeCodeResponse>();
        return response;
    }
}
