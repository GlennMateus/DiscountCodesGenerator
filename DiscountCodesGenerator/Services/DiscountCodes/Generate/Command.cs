namespace DiscountCodesGenerator.Services.DiscountCodes.Generate;

public record Command(ushort Count, byte Length) : IRequest<Result>;