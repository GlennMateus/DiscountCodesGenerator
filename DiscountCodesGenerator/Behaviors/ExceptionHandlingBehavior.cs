using Grpc.Core.Interceptors;

namespace DiscountCodesGenerator.Behaviors;

public class ExceptionHandlingBehavior : Interceptor
{
    private readonly ILogger<ExceptionHandlingBehavior> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");

            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred"));
        }
    }
}