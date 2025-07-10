using DiscountCodesGenerator.Services.DiscountCodes.GetDiscountCodesService;

namespace DiscountCodesGenerator.Configuration;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<IEnumerable<DiscountCode>, GetDiscountCodesResponse>
            .NewConfig()
            .Map(dest => dest.Codes, src => src.Select(dc => dc.Code));
    }
}
