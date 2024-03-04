using CurrencyConverter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.API.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public CurrencyController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet("quotes/{baseCurrency}/{quoteCurrency}/{amount:decimal}")]
        public async Task<IActionResult> GetQuote(string baseCurrency, string quoteCurrency, decimal amount)
        {
            var quote = await _quoteService.GetQuoteAsync(baseCurrency, quoteCurrency, amount);

            if (quote is null)
            {
                return NotFound();
            }

            return Ok(quote);
        }
    }
}
