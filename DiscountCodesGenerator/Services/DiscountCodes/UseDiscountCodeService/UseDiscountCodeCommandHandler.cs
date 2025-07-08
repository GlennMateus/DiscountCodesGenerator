using DiscountCodesGenerator.Repositories.DiscountCodeRespository;

namespace DiscountCodesGenerator.Services.DiscountCodes.UseDiscountCodeService;

public record UseCodeCommand(string Code) : IRequest<UseCodeResult>;
public record UseCodeResult(bool success);

public class UseDiscountCodeCommandHandler(IDiscountCodeRepository _repository
    , ILogger<UseDiscountCodeCommandHandler> _logger)
    : IRequestHandler<UseCodeCommand, UseCodeResult>
{
    public async Task<UseCodeResult> Handle(UseCodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔧 Executing discount code usage: {Code} ...", request.Code);

        try
        {
            DiscountCode dbCode = await _repository.GetCodeAsync(request.Code, cancellationToken);
            if (dbCode != null)
            {
                dbCode.TimesUsed++;
                await _db.SaveChangesAsync(cancellationToken);

                return new UseCodeResult(true);
            }
            return new UseCodeResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Unhandled error has occurred. Please contact an administrator.");
            throw;
        }
    }
}
