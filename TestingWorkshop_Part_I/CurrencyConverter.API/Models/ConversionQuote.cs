namespace CurrencyConverter.API.Models
{
    public class ConversionQuote
    {
        public string BaseCurrency { get; init; } = default!;
        public string QuoteCurrency { get; init; } = default!;
        public decimal BaseAmount { get; set; }
        public decimal QuoteAmount { get; set; }
    }
}
