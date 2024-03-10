namespace CurrencyConverter.API.Logger
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message, params object?[] args);
    }
    
}
