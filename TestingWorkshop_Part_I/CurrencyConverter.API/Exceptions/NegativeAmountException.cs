namespace CurrencyConverter.API.Exceptions
{
    public class NegativeAmountException : ValidationException
    {
        public NegativeAmountException()
            : base("Amount", $"You can only convert a positive amount of money")
        {

        }
    }
}
