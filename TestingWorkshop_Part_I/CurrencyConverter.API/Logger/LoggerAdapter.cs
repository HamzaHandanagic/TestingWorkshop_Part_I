
namespace CurrencyConverter.API.Logger
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object?[] args)
        {
          _logger.LogInformation(message, args);
        }
    }
}
