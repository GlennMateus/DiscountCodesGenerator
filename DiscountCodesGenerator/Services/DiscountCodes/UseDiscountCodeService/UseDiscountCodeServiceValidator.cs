namespace DiscountCodesGenerator.Services.DiscountCodes.UseDiscountCodeService;

public class UseDiscountCodeServiceValidator : AbstractValidator<UseCodeCommand>
{
    public UseDiscountCodeServiceValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .WithMessage("Code is mandatory!")
            .Length(7, 8)
            .WithMessage("Code length must be between 7 and 8.");
    }
}
