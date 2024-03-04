using Microsoft.AspNetCore.Connections;

namespace CurrencyConverter.API.Repositories
{
    public class RatesRepository : IRatesRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RatesRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CurrencyRate?> GetRateAsync(string baseCurrency, string quoteCurrency)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<CurrencyRate?>(
                "SELECT * from CurrencyRates WHERE FromCurrency=@baseCurrency AND ToCurrency=@quoteCurrency" +
                " ORDER BY TimestampUtc DESC LIMIT 1", new { baseCurrency, quoteCurrency });
        }
    }
}
