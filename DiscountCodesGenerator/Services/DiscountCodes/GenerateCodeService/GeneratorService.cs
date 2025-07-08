namespace DiscountCodesGenerator.Services.DiscountCodes.GenerateCodeService;

public class GeneratorService(IMediator sender)
    : Generator.GeneratorBase
{

    public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request
        , ServerCallContext context)
    {
        var command = request.Adapt<GenerateCodesCommand>();
        var result = await sender.Send(command);
        var response = new GenerateResponse();
        response.Codes.AddRange(result.Codes);

        return response;
    }
}
