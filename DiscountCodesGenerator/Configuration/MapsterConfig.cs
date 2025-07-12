using DiscountCodesGenerator.Services.DiscountCodes.Get;

namespace DiscountCodesGenerator.Configuration;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<IEnumerable<DiscountCode>, Result>
            .NewConfig()
            .Map(dest => dest.Codes, src => src.Select(dc => dc.Code));
    }
}
