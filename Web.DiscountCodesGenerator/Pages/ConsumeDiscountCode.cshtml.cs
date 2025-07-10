using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Web.DiscountCodesGenerator.Models;
using Web.DiscountCodesGenerator.Services;

namespace Web.DiscountCodesGenerator.Pages
{
    public class ConsumeDiscountCodeModel : PageModel
    {
        private readonly ILogger<ConsumeDiscountCodeModel> _logger;
        private readonly IGrpcDiscountCodesClient _grpcClient;
        [BindProperty]
        public ConsumeCodeModel ConsumeCode { get; set; }
        public bool ShowResult { get; set; }
        public string GeneratedCodes { get; set; }

        public ConsumeDiscountCodeModel(ILogger<ConsumeDiscountCodeModel> logger, IGrpcDiscountCodesClient grpcClient)
        {
            _logger = logger;
            _grpcClient = grpcClient;
        }
        public async Task<IActionResult> OnGet()
        {
            await LoadCodes();
            return Page();
        }

        public async Task<IActionResult> OnPostConsume()
        {
            if (!TryValidateModel(ConsumeCode, nameof(ConsumeCode)))
            {
                return Page();
            }
            var response = await _grpcClient.ConsumeCodeAsync(ConsumeCode.Code);
            if (response.Success)
            {
                await LoadCodes();
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
}
