namespace DiscountCodesGenerator.Services.DiscountCodes.Consume;

public class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .WithMessage("Code is mandatory!")
            .Length(7, 8)
            .WithMessage("Code length must be between 7 and 8.");
    }
}
