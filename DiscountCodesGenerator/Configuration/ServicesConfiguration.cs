using DiscountCodesGenerator.Repositories.DiscountCodeRespository;

namespace DiscountCodesGenerator.Configuration;

public static class ServicesConfiguration
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("SQL")));

        services.AddPooledDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQL"));
        });

        services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();

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
