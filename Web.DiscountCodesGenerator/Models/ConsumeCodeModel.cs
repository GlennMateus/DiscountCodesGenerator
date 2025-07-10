using System.ComponentModel.DataAnnotations;

namespace Web.DiscountCodesGenerator.Models;

public class ConsumeCodeModel
{
    [Required(ErrorMessage = "Code values is required")]
    [StringLength(8, MinimumLength = 7, ErrorMessage = "Code must be 7 or 8 characters.")]
    public string Code { get; set; }
}
