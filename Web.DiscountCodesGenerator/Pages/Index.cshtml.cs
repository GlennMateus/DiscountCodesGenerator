using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Web.DiscountCodesGenerator.Models;
using Web.DiscountCodesGenerator.Services;

namespace Web.DiscountCodesGenerator.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IGrpcDiscountCodesClient _grpcClient;

    [BindProperty]
    public AddCodesModel AddCodes { get; set; }

    public bool ShowResult { get; set; }
    public string GeneratedCodes { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IGrpcDiscountCodesClient grpcClient)
    {
        _logger = logger;
        _grpcClient = grpcClient;
    }

    public async Task<IActionResult> OnGet()
    {
        await LoadCodes();
        return Page();
    }

    public async Task<IActionResult> OnPostGenerate()
    {
        if (!TryValidateModel(AddCodes, nameof(AddCodes)))
        {
            return Page();
        }
        var response = await _grpcClient.GenerateCodesAsync(AddCodes.Count, AddCodes.Length);
        if (response.Codes.Any())
        {
            TempData["ToastMessage"] = "Codes created successfully";
            TempData["ToastType"] = "success";
            await LoadCodes();
        }
        else
        {
            TempData["ToastMessage"] = "Something went wrong...";
            TempData["ToastType"] = "error";
        }
        return Page();
    }

    private async Task LoadCodes()
    {
        var getCodesResponse = await _grpcClient.GetDiscountCodesAsync();
        if (getCodesResponse.Codes.Any())
        {
            ShowResult = true;
            GeneratedCodes = JsonSerializer.Serialize(getCodesResponse, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
