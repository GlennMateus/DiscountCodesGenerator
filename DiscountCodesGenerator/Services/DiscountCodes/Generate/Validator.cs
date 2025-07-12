namespace DiscountCodesGenerator.Services.DiscountCodes.Generate;

public class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(c => c.Length)
            .InclusiveBetween((byte)7, (byte)8)
            .WithMessage("Length must be between 7 and 8.");

        RuleFor(x => x.Count)
            .InclusiveBetween((ushort)1, (ushort)2000)
            .WithMessage("Count must be between 1 and 2000.");
    }
}
