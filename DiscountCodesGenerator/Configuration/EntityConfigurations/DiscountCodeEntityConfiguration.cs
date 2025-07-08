using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscountCodesGenerator.Configuration.EntityConfigurations;

public class DiscountCodeEntityConfiguration : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
