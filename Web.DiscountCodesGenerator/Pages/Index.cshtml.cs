using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web.DiscountCodesGenerator.Services;

namespace Web.DiscountCodesGenerator.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly GrpcDiscountCodesClient _grpcClient;

    [BindProperty]
    [Required(ErrorMessage = "Count value is required.")]
    [Range(1, 2000, ErrorMessage = "Value must be between 1 and 2000.")]
    public uint Count { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Length value is required")]
    [Range(7, 8, ErrorMessage = "Length must be between 7 and 8.")]
    public byte Length { get; set; }

    [BindProperty]
    public bool ShowResult { get; set; }
    [BindProperty]
    public string GeneratedCodes { get; set; }

    public IndexModel(ILogger<IndexModel> logger, GrpcDiscountCodesClient grpcClient)
    {
        _logger = logger;
        _grpcClient = grpcClient;
    }

    public void OnGet()
    {
        
    }

    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> OnPostGenerate()
    {
        var response = await _grpcClient.GenerateCodes(Count, Length);
        return RedirectToPage();
    }
}
