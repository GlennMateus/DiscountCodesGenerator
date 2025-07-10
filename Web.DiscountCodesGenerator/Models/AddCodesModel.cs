using System.ComponentModel.DataAnnotations;

namespace Web.DiscountCodesGenerator.Models;

public class AddCodesModel
{
    [Required(ErrorMessage = "Count value is required.")]
    [Range(1, 2000, ErrorMessage = "Value must be between 1 and 2000.")]
    public uint Count { get; set; }

    [Required(ErrorMessage = "Length value is required")]
    [Range(7, 8, ErrorMessage = "Length must be between 7 and 8.")]
    public byte Length { get; set; }
}
