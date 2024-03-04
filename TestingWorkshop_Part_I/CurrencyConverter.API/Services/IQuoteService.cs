using CurrencyConverter.API.Models;

namespace CurrencyConverter.API.Services
{
    public interface IQuoteService
    {
        Task<ConversionQuote> GetQuoteAsync(string baseCurrency, string quoteCurrency, decimal amount);
    }
}
