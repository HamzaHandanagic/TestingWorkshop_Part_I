namespace CurrencyConverter.API.Repositories
{
    public interface IRatesRepository
    {
        Task<CurrencyRate> GetRateAsync(string baseCurrency, string quoteCurrency);
    }
}
