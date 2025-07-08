using System.ComponentModel.DataAnnotations;

namespace DiscountCodesGenerator.Entities;

public class DiscountCode
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(8)]
    public string Code { get; set; }
    public int TimesUsed { get; set; } = 0;
}
