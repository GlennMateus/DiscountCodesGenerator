namespace DiscountCodesGenerator.Services.DiscountCodes.Generate;

public class GeneratorService(IMediator sender)
    : GenerateCodesService.GenerateCodesServiceBase
{

    public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request
        , ServerCallContext context)
    {
        var command = request.Adapt<Command>();
        var result = await sender.Send(command);
        var response = new GenerateResponse();
        response.Codes.AddRange(result.Codes);

        return response;
    }
}
