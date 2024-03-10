using CurrencyConverter.API.Database;
using CurrencyConverter.API.Exceptions;
using CurrencyConverter.API.Logger;
using CurrencyConverter.API.Models;
using CurrencyConverter.API.Repositories;
using CurrencyConverter.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.API.Tests.Unit
{
    public class QuoteServiceTests
    {
        private readonly QuoteService _sut;
		private readonly IRatesRepository _ratesRepository = NSubstitute.Substitute.For<IRatesRepository>();
		private ILoggerAdapter<QuoteService> _logger = Substitute.For<ILoggerAdapter<QuoteService>>();

        public QuoteServiceTests()
        {
			_sut = new(_ratesRepository, _logger);
        }

        [Fact]
		public async Task GetQuoteAsync_ReturnsQuote_WhenCurrenciesAreValid()
		{
			// Arrange
			var fromCurrency = "USD";
			var toCurrency = "BAM";
			var amount = 100;

			var expectedRate = new CurrencyRate()
			{
				FromCurrency = fromCurrency,
				ToCurrency = toCurrency,
				Rate = 1.82m
			};

			_ratesRepository.GetRateAsync(fromCurrency, toCurrency).Returns(expectedRate);

			var expectedQuote = new ConversionQuote()
			{
				BaseCurrency = fromCurrency,
				QuoteCurrency = toCurrency,
				BaseAmount = amount,
				QuoteAmount = 182.000M
            };

			// Act
			var quote = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

			// Assert
			quote.Should().BeEquivalentTo(expectedQuote);
		}


		[Fact]
		public async Task GetQuoteAsync_ShouldReturnNull_WhenCurrenciesNotSupported()
		{
            // Arrange
            var fromCurrency = "DKK";
            var toCurrency = "BAM";
            var amount = 100;

            _ratesRepository.GetRateAsync(fromCurrency, toCurrency).ReturnsNull();

            // Act
            var quote = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

			// Assert
			quote.Should().BeNull();
        }


		[Fact]
		public async Task GetQuoteAsync_ShouldThrowSameCurrencyException_WhenSameCurrenciesAreUsed()
		{
            // Arrange
            var fromCurrency = "BAM";
            var toCurrency = "BAM";
            var amount = 100;

			// Act
			var resultAction = () => _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

			// Assert
			await resultAction.Should().ThrowAsync<SameCurrencyException>().WithMessage($"You cannot convert currency {fromCurrency} to itself");
        }


		[Fact]
		public async Task GetQuoteAsync_ShouldLogAppropriateMessage_WhenInvoked()
		{
            // Arrange
            var fromCurrency = "USD";
            var toCurrency = "BAM";
            var amount = 100;

            var expectedRate = new CurrencyRate()
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Rate = 1.82m
            };

            _ratesRepository.GetRateAsync(fromCurrency, toCurrency).Returns(expectedRate);

            // Act
            await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

			// Assert
			_logger.Received(1).LogInformation("Retrieved quote for currencies {FromCurrency}->{ToCurrency}", Arg.Is<object[]>
												(x => x[0].ToString() == fromCurrency && 
													  x[1].ToString() == toCurrency));
        }
    }
}
