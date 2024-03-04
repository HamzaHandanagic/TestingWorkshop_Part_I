using CurrencyConverter.API.Exceptions;
using CurrencyConverter.API.Models;
using CurrencyConverter.API.Repositories;
using System.Diagnostics;

namespace CurrencyConverter.API.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IRatesRepository _ratesRepository;
        private readonly ILogger _logger;

        public QuoteService(IRatesRepository ratesRepository, ILogger logger)
        {
            _ratesRepository = ratesRepository;
            _logger = logger;
        }

        public async Task<ConversionQuote> GetQuoteAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (amount <= 0)
                {
                    throw new NegativeAmountException();
                }

                if (fromCurrency == toCurrency)
                {
                    throw new SameCurrencyException(fromCurrency);
                }

                var rate = await _ratesRepository.GetRateAsync(fromCurrency, toCurrency);

                if (rate is null)
                {
                    return null;
                }

                var quoteAmount = rate.Rate * amount;

                return new ConversionQuote
                {
                    BaseCurrency = fromCurrency,
                    QuoteCurrency = toCurrency,
                    BaseAmount = amount,
                    QuoteAmount = quoteAmount
                };
            }
            finally
            {
                _logger.LogInformation(
                    "Retrieved quote for currencies {FromCurrency}->{ToCurrency} in {ElapsedMilliseconds}ms",
                    fromCurrency, toCurrency, sw.ElapsedMilliseconds);
            }
        }
    }
}
