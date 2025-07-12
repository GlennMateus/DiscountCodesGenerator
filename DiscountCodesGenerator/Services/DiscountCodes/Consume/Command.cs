namespace DiscountCodesGenerator.Services.DiscountCodes.Consume;

public record Command(string Code) : IRequest<Result>;