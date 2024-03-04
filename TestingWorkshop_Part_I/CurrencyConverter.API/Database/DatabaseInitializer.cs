using Dapper;
using Microsoft.AspNetCore.Connections;

namespace CurrencyConverter.API.Database
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var created = await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS CurrencyRates (
                    FromCurrency TEXT NOT NULL, 
                    ToCurrency TEXT NOT NULL,
                    Rate DECIMAL NOT NULL,
                    TimestampUtc timestamp  without time zone default (now() at time zone 'utc'),
                    PRIMARY KEY(FromCurrency, ToCurrency))");

            await SeedCurrencyRates();
        }

        /// <summary>
        /// BAM => EUR, USD, GDP
        /// EUR, USD, GDP => BAM
        /// </summary>
        public async Task SeedCurrencyRates()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('BAM', 'USD', 0.5501, NOW()::timestamp) ON CONFLICT DO NOTHING;" +
                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('BAM', 'EUR', 0.5112, NOW()::timestamp) ON CONFLICT DO NOTHING;" +
                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('BAM', 'GDP', 0.4401, NOW()::timestamp) ON CONFLICT DO NOTHING;" +

                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('USD', 'BAM', 1.8204, NOW()::timestamp) ON CONFLICT DO NOTHING;" +
                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('EUR', 'BAM', 1.9606, NOW()::timestamp) ON CONFLICT DO NOTHING;" +
                "INSERT INTO CurrencyRates (FromCurrency, ToCurrency, Rate, TimestampUtc) VALUES ('GDP', 'BAM', 2.2764, NOW()::timestamp) ON CONFLICT DO NOTHING;"
                );
        }
    }
}
