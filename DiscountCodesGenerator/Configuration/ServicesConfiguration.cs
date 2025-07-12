using DiscountCodesGenerator.Tools.NanoIdGenerator;

namespace DiscountCodesGenerator.Configuration;

public static class ServicesConfiguration
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPooledDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQL"));
        });

        services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
        services.AddSingleton<IIdGenerator, IdGenerator>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionHandlingBehavior>();
        });

        services.AddGrpcReflection();
    }
}
